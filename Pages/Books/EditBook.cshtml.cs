using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Driver;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages;

public class EditBook : PageModel
{
    private BookService _bookService;
    
    // Если взяли книгу на редактирование
    public Book Book { get; set; }
    
    public EditBook(BookService bookService)
    {
        _bookService = bookService;
    }

    public async Task OnGet(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            this.Book = await _bookService.GetById(id);
        }
        else
        {
            this.Book = new Book();    
        }
    }
    
    public async Task<IActionResult> OnPost(BookUpdateViewModel bookViewModel)
    {
        if (bookViewModel.Id == ObjectId.Empty.ToString())
        {
            await _bookService.AddBook(bookViewModel.ToBook());
        }
        else
        {
            await _bookService.EditBook(bookViewModel.ToBook());
        }
        
        return RedirectToPage("books");
    }
}