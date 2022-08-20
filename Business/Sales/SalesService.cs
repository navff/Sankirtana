using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.Sales;

public class SalesService
{
    private readonly DbStore _dbStore;

    public SalesService(DbStore dbStore)
    {
        _dbStore = dbStore;
    }

    public async Task<Sale> AddSale(Sale sale)
    {
        sale.Id =ObjectId.GenerateNewId();
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        await sales.InsertOneAsync(sale);
        Console.WriteLine("ID:" + sale.Id);
        
        return sale;
    }

    public async Task<List<Sale>> GetSales()
    {
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        var result = await sales.Find(_ => true).Sort(Builders<Sale>.Sort.Descending(nameof(Sale.Date))).ToListAsync();
        return result.ToList();
    }

    public async Task DeleteSale(string id)
    {
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        await sales.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<Sale> GetById(string id)
    {
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        var sale = await sales.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        return sale;
    }

    public async Task<Sale> EditSale(Sale sale)
    {
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        await sales.ReplaceOneAsync(new BsonDocument("_id", sale.Id), sale);
        return sale;
    }
}