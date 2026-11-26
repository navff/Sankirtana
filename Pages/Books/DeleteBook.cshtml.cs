using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages;

[RESTAuthorize(PortalRoles.Administrator)]
[AutoValidateAntiforgeryToken(Order = 1100)]
public class DeleteBook : PageModel
{
    private readonly BookService _bookService;

    public DeleteBook(BookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult OnGet() => RedirectToPage("/Books/Index");

    public async Task<IActionResult> OnPost(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return BadRequest();
        await _bookService.DeleteBook(id);
        return RedirectToPage("/Books/Index");
    }
}
