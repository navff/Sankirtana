using Microsoft.AspNetCore.Mvc;
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
        var books = _dbStore.DB.GetCollection<Book>("books");
        var result = await books.FindAsync(_ => true);
        return result.ToList();
    }

    public async Task DeleteBook(string id)
    {
        var books = _dbStore.DB.GetCollection<Book>("books");
        await books.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<Book> GetById(string id)
    {
        var books = _dbStore.DB.GetCollection<Book>("books");
        var book = await books.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        return book;
    }

    public async Task<Book> EditBook(Book book)
    {
        var books = _dbStore.DB.GetCollection<Book>("books");
        await books.ReplaceOneAsync(new BsonDocument("_id", book.Id), book);
        return book;
    }
}