using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Book;

namespace Sankirtana.Web.Pages;

public class DeleteBook : PageModel
{
    private readonly BookService _bookService;

    public DeleteBook(BookService bookService)
    {
        _bookService = bookService;
    }

    public void OnGet()
    {
        
    }

    public async Task OnPost(string id)
    {
        await _bookService.DeleteBook(id);
    }
}