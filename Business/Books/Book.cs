using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sankirtana.Web.Business.Books;


public class Book
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [Display(Name = "Название книги")]
    public string Name { get; set; }
    
    [Display(Name = "Начисляемые очки")]
    public int VolumePoints { get; set; }
    
    public string Category { get; set; }
}