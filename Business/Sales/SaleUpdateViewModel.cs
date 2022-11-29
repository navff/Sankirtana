using MongoDB.Bson;
using Sankirtana.Web.Business.Books;

namespace Sankirtana.Web.Business.Sales;

public class SaleUpdateViewModel
{
    public string Id { get; set; }
    
    public string ContactName { get; set; }
    
    public string ContactPhone { get; set; }

    public string BookId { get; set; }
    
    public int Count { get; set; }
    
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
}