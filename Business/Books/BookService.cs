﻿using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Business.Books;

public class BookService
{
    private readonly DbStore _dbStore;

    public BookService(DbStore dbStore)
    {
        _dbStore = dbStore;
    }

    public async Task<Books.Book> AddBook(Books.Book book)
    {
        book.Id =ObjectId.GenerateNewId();
        var books = _dbStore.DB.GetCollection<Books.Book>("books");
        await books.InsertOneAsync(book);
        Console.WriteLine("ID:" + book.Id);
        
        return book;
    }

    public async Task<List<Books.Book>> GetBooks()
    {
        var books = _dbStore.DB.GetCollection<Books.Book>("books");
        var result = await books.Find(_ => true).Sort(Builders<Books.Book>.Sort.Ascending(nameof(Books.Book.Name))).ToListAsync();
        return result.ToList();
    }

    public async Task DeleteBook(string id)
    {
        var books = _dbStore.DB.GetCollection<Books.Book>("books");
        await books.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
    
    public async Task<Books.Book> GetById(string id)
    {
        var books = _dbStore.DB.GetCollection<Books.Book>("books");
        var book = await books.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        return book;
    }

    public async Task<Books.Book> EditBook(Books.Book book)
    {
        var books = _dbStore.DB.GetCollection<Books.Book>("books");
        await books.ReplaceOneAsync(new BsonDocument("_id", book.Id), book);
        return book;
    }
}