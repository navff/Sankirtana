using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.PortalUsers;

public class PortalUserService
{
    private readonly DbStore _dbStore;
    private readonly InitialAdministratorSetup _initialAdministrators;

    public PortalUserService(DbStore dbStore, InitialAdministratorSetup initialAdministrators)
    {
        _dbStore = dbStore;
        _initialAdministrators = initialAdministrators;
    }
    
    public async Task<PortalUsers.PortalUser> AddUser(PortalUsers.PortalUser user)
    {
        user.Id = ObjectId.GenerateNewId();
        user.Token = Guid.NewGuid().ToString();
        user.RoleAssignedByAdministrator = true;
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        await users.InsertOneAsync(user);
        Console.WriteLine("ID:" + user.Id);
        
        return user;
    }

    public async Task<List<PortalUsers.PortalUser>> GetUsers()
    {
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        
        var result = await users.Find(_ => true)
            .Sort(Builders<PortalUsers.PortalUser>.Sort.Ascending(nameof(PortalUsers.PortalUser.LastName)))
            .ToListAsync();
        
        return result.ToList();
    }

    public async Task DeleteUser(string id)
    {
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        await users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<PortalUsers.PortalUser> GetById(string id)
    {
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        var user = await users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        return user;
    }
    
    public async Task<PortalUsers.PortalUser?> GetByToken(string token)
    {
        await _initialAdministrators.EnsureAsync();
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        return await users.Find(u => u.Token == token).FirstOrDefaultAsync();
    }

    public async Task<PortalUsers.PortalUser> EditUser(PortalUsers.PortalUser user)
    {
        var oldUser = await GetById(user.Id.ToString());

        if (!string.IsNullOrEmpty(oldUser.Token)
            && string.IsNullOrEmpty(user.Token))
        {
            user.Token = oldUser.Token;
        }

        if (string.IsNullOrEmpty(user.Token))
        {
            user.Token = Guid.NewGuid().ToString();
        }

        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        var update = Builders<PortalUser>.Update
            .Set(existing => existing.FirstName, user.FirstName)
            .Set(existing => existing.LastName, user.LastName)
            .Set(existing => existing.City, user.City)
            .Set(existing => existing.Token, user.Token)
            .Set(existing => existing.Role, user.Role)
            .Set(existing => existing.RoleAssignedByAdministrator, true);
        await users.UpdateOneAsync(existing => existing.Id == user.Id, update);
        return user;
    }
}
