using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Business.Sales;

public class Sale
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    public PortalUserShort User { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date { get; set; }

    public int Count { get; set; } = 1;
    
    public string ContactName { get; set; }
    
    public string ContactPhone { get; set; }

    public Book Book { get; set; }

    public decimal VolumePoints => this.Book.VolumePoints * this.Count;
}
