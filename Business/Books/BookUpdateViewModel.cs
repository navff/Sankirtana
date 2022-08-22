using MongoDB.Bson;

namespace Sankirtana.Web.Business.Books;

public class BookUpdateViewModel
{
    public string Id { get; set; }

    public string Name { get; set; } = "";
    
    public string Category { get; set; }

    public Books.Book ToBook()
    {
        return new Books.Book()
        {
            Id = string.IsNullOrEmpty(this.Id) 
                ? new ObjectId() 
                : new ObjectId(this.Id),
            Name = this.Name,
            VolumePoints = Book.GetVolumeByCategory(this.Category),
            Category = this.Category
        };
    }
}