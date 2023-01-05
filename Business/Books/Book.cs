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
    public decimal VolumePoints { get; set; }
    
    public string Category { get; set; }

    public static decimal GetVolumeByCategory(string category)
    {
        return category switch
        {
            "MahaBig" => 2,
            "Big" => 1,
            "Medium" => 0.5m,
            "Small" => 0.25m,
            _ => 0
        };
    }
}

public static class BookCategory
{
    public const string MahaBig = "MahaBig";
    public const string Big = "Big";
    public const string Medium = "Medium";
    public const string Small = "Small";
}