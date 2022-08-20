using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages.Books;

public class IndexBooksPageModel : PageModel
{
    private readonly BookService _bookService;
    public List<Book> BookList = new List<Book>();

    public IndexBooksPageModel(BookService bookService)
    {
        _bookService = bookService;
    }

    public async Task OnGet()
    {
        BookList = await _bookService.GetBooks();
    }
}