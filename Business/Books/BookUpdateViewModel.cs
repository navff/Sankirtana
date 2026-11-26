using MongoDB.Bson;

namespace Sankirtana.Web.Business.Books;

using System.ComponentModel.DataAnnotations;

public class BookUpdateViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Введите название книги.")]
    [Display(Name = "Название книги")]
    public string Name { get; set; } = "";

    [Display(Name = "Категория")]
    public string? Category { get; set; }

    [Display(Name = "Баллы")]
    public string? VolumePoints { get; set; }

    public Book ToBook(ObjectId id, decimal? volumePoints)
    {
        return new Book
        {
            Id = id,
            Name = Name,
            VolumePoints = volumePoints ?? Book.GetVolumeByCategory(Category ?? ""),
            Category = Category ?? ""
        };
    }
}
