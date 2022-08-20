using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.PortalUser;

public class PortalUserService
{
    private readonly DbStore _dbStore;

    public PortalUserService(DbStore dbStore)
    {
        _dbStore = dbStore;
    }
    
    public async Task<PortalUser> AddUser(PortalUser user)
    {
        user.Id =ObjectId.GenerateNewId();
        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        await users.InsertOneAsync(user);
        Console.WriteLine("ID:" + user.Id);
        
        return user;
    }

    public async Task<List<PortalUser>> GetUsers()
    {
        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        
        var result = await users.Find(_ => true)
            .Sort(Builders<PortalUser>.Sort.Ascending(nameof(PortalUser.LastName)))
            .ToListAsync();
        
        return result.ToList();
    }

    public async Task DeleteUser(string id)
    {
        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        await users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<PortalUser> GetById(string id)
    {
        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        var user = await users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        return user;
    }

    public async Task<PortalUser> EditUser(PortalUser user)
    {
        var users = _dbStore.DB.GetCollection<PortalUser>("users");
        await users.ReplaceOneAsync(new BsonDocument("_id", user.Id), user);
        return user;
    }
}