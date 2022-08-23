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
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Date { get; set; }
    
    public string ContactName { get; set; }
    
    public string ContactPhone { get; set; }

    public Book Book { get; set; }
}
