using MongoDB.Bson;

namespace Sankirtana.Web.Business.Book;

public class BookUpdateViewModel
{
    public string Id { get; set; }

    public string Name { get; set; } = "";
    
    public int VolumePoints { get; set; }

    public Book ToBook()
    {
        return new Book()
        {
            Id = string.IsNullOrEmpty(this.Id) 
                ? new ObjectId() 
                : new ObjectId(this.Id),
            Name = this.Name,
            VolumePoints = this.VolumePoints
        };
    }
}