using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.PortalUsers;

public sealed class InitialAdministratorSetup
{
    private readonly DbStore _dbStore;
    private readonly ILogger<InitialAdministratorSetup> _logger;
    private readonly string[] _names;
    private readonly SemaphoreSlim _gate = new(1);
    private volatile bool _initialized;

    public InitialAdministratorSetup(DbStore dbStore, IConfiguration configuration,
        ILogger<InitialAdministratorSetup> logger)
    {
        _dbStore = dbStore;
        _logger = logger;
        _names = configuration.GetSection("InitialAdministrators").Get<string[]>() ?? Array.Empty<string>();
    }

    public async Task EnsureAsync()
    {
        if (_initialized) return;

        await _gate.WaitAsync();
        try
        {
            if (_initialized) return;

            foreach (var name in _names)
            {
                await AssignAsync(name);
            }
            _initialized = true;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task AssignAsync(string name)
    {
        var key = NormalizeName(name);
        if (string.IsNullOrEmpty(key)) return;

        var assignments = _dbStore.DB.GetCollection<Assignment>("portal_initial_administrators");
        var assignment = await assignments.Find(entry => entry.Name == key).FirstOrDefaultAsync();
        if (assignment?.Applied == true) return;

        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        if (assignment == null)
        {
            var names = await users.Find(Builders<PortalUser>.Filter.Empty)
                .Project(user => new { user.Id, user.FirstName, user.LastName })
                .ToListAsync();
            var matches = names.Where(user => NormalizeName($"{user.FirstName} {user.LastName}") == key)
                .Take(2).ToList();
            if (matches.Count != 1)
            {
                _logger.LogWarning("Initial administrator {Name} requires exactly one matching account.", name);
                return;
            }

            assignment = new Assignment { Name = key, UserId = matches[0].Id };
            try
            {
                await assignments.InsertOneAsync(assignment);
            }
            catch (MongoWriteException error) when (error.WriteError?.Category == ServerErrorCategory.DuplicateKey)
            {
                assignment = await assignments.Find(entry => entry.Name == key).FirstAsync();
            }
        }

        // Keep retries from restoring an administrator role changed after the initial assignment.
        var userFilters = Builders<PortalUser>.Filter;
        await users.UpdateOneAsync(
            userFilters.Eq(user => user.Id, assignment.UserId) &
            userFilters.Ne(user => user.RoleAssignedByAdministrator, true),
            Builders<PortalUser>.Update.Set(user => user.Role, PortalRoles.Administrator)
                .Set(user => user.RoleAssignedByAdministrator, true));
        await assignments.UpdateOneAsync(entry => entry.Name == key,
            Builders<Assignment>.Update.Set(entry => entry.Applied, true));
    }

    private static string NormalizeName(string name) =>
        string.Join(' ', name.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)).ToUpperInvariant();

    private sealed class Assignment
    {
        [BsonId]
        public string Name { get; set; } = "";
        public ObjectId UserId { get; set; }
        public bool Applied { get; set; }
    }
}
