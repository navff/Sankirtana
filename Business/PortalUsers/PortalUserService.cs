using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.PortalUsers;

public class PortalUserService
{
    private readonly DbStore _dbStore;

    public PortalUserService(DbStore dbStore)
    {
        _dbStore = dbStore;
    }
    
    public async Task<PortalUsers.PortalUser> AddUser(PortalUsers.PortalUser user)
    {
        user.Id =ObjectId.GenerateNewId();
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

    public async Task<PortalUsers.PortalUser> EditUser(PortalUsers.PortalUser user)
    {
        var users = _dbStore.DB.GetCollection<PortalUsers.PortalUser>("users");
        await users.ReplaceOneAsync(new BsonDocument("_id", user.Id), user);
        return user;
    }
}