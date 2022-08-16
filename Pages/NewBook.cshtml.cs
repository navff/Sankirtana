using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages;

public class NewBook : PageModel
{
    private BookService _bookService;
    
    public NewBook(BookService bookService)
    {
        _bookService = bookService;
    }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost(Book book)
    {
        await _bookService.AddBook(book);
        return RedirectToPage("books");
    }
}