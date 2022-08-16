using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.Book;

public class BookService
{
    private readonly DbStore _dbStore;

    public BookService(DbStore dbStore)
    {
        _dbStore = dbStore;
    }

    public async Task<Book> AddBook(Book book)
    {
        book.Id =ObjectId.GenerateNewId();
        var books = _dbStore.DB.GetCollection<Book>("books");
        await books.InsertOneAsync(book);
        Console.WriteLine("ID:" + book.Id);
        
        return book;
    }

    public async Task<List<Book>> GetBooks()
    {
        var builder = new FilterDefinitionBuilder<Book>();
        var filter = builder.Empty;
        
        var books = _dbStore.DB.GetCollection<Book>("books");
        var count = books.CountDocuments(filter);
        var result = await books.FindAsync(_ => true);
        return result.ToList();
    }
}