using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages.Sales;

public class AddSale : PageModel
{
    public List<Book> BookList;
    private BookService _bookService;

    public AddSale(BookService bookService)
    {
        _bookService = bookService;
    }

    public async Task OnGet()
    {
        this.BookList = await _bookService.GetBooks();
    }
}