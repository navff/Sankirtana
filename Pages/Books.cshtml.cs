using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages;

public class Books : PageModel
{
    private readonly BookService _bookService;
    public List<Book> BookList = new List<Book>();

    public Books(BookService bookService)
    {
        _bookService = bookService;
    }

    public async Task OnGet()
    {
        BookList = await _bookService.GetBooks();
    }
}