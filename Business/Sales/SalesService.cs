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

    public async Task<List<Sale>> GetSalesByUser(string userId, DateTime? date)
    {
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        
        var builder = Builders<Sale>.Filter;
        var filter = builder.Empty;
        
        filter &= builder.Eq(x => x.User.Id, new ObjectId(userId));
        
        if (date.HasValue)
        {
            filter &= builder.Gt(x => x.Date, date.Value) ;
            filter &= builder.Lt(x => x.Date, date.Value.AddDays(1)) ;
        }

        var result = await sales.Find(filter)
            .Sort(Builders<Sale>.Sort.Descending(nameof(Sale.Date)))
            .ToListAsync();
        return result.ToList();
    }
    
    public async Task<List<UserSalesStatisticRecord>> GetSalesStat(
        DateTime? dateStart, 
        DateTime? dateEnd)
    {
        var builder = Builders<Sale>.Filter;
        var filter = builder.Empty;
        
        if (dateStart.HasValue)
        {
            filter &= builder.Gte(x => x.Date, dateStart.Value) ;
        }
        
        if (dateEnd.HasValue)
        {
            filter &= builder.Lte(x => x.Date, dateEnd.Value) ;
        }
        
        var sales = _dbStore.DB.GetCollection<Sale>("sales");
        var filteredSales = await sales.Find(filter)
            .Sort(Builders<Sale>.Sort.Descending(nameof(Sale.Date)))
            .ToListAsync();

        var result = new List<UserSalesStatisticRecord>();
        foreach (var sale in filteredSales.DistinctBy(s => s.User.Id))
        {
            var userSales = new List<BookSaleStatisticRecord>();
            foreach (var uniqSale in filteredSales.DistinctBy(s => s.Book.Id))
            {
                var bookSalesCount = filteredSales.Count(s => s.Book.Id == uniqSale.Book.Id);
                var volumePoints = bookSalesCount * uniqSale.Book.VolumePoints;
                
                if (userSales.Any(s => s.BookName == uniqSale.Book.Name)) continue;
                
                userSales.Add(new BookSaleStatisticRecord
                {
                    User = sale.User,
                    BookName = uniqSale.Book.Name,
                    SaleId = sale.Id.ToString(),
                    Quantity = bookSalesCount,
                    VolumePoints = volumePoints
                });
            }
            
            result.Add(new UserSalesStatisticRecord()
            {
                User = sale.User,
                Sales = userSales
            });
        }
        
        return result;
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