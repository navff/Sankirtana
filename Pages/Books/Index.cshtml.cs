using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages.Books;

[RESTAuthorize]
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