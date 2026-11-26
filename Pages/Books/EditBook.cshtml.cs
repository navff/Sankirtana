using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages;

[RESTAuthorize(PortalRoles.Administrator)]
[AutoValidateAntiforgeryToken(Order = 1100)]
public class EditBook : PageModel
{
    private readonly BookService _bookService;

    public EditBook(BookService bookService)
    {
        _bookService = bookService;
    }

    [BindProperty]
    public BookUpdateViewModel Input { get; set; } = new();
    public bool IsNew => string.IsNullOrEmpty(Input.Id);

    public async Task<IActionResult> OnGet(string? id)
    {
        if (string.IsNullOrEmpty(id)) return Page();
        if (!ObjectId.TryParse(id, out _)) return BadRequest();

        var book = await _bookService.GetById(id);
        if (book == null) return NotFound();

        Input = new BookUpdateViewModel
        {
            Id = book.Id.ToString(),
            Name = book.Name,
            Category = book.Category,
            VolumePoints = book.VolumePoints.ToString(CultureInfo.InvariantCulture)
        };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var id = ObjectId.Empty;
        if (!IsNew && !ObjectId.TryParse(Input.Id, out id)) return BadRequest();

        decimal? points = null;
        if (!string.IsNullOrWhiteSpace(Input.VolumePoints))
        {
            if (decimal.TryParse(Input.VolumePoints.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var value))
            {
                points = value;
            }
            else
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.VolumePoints)}",
                    "Введите ноль или положительное число, например 0,25.");
            }
        }

        if (!ModelState.IsValid) return Page();

        if (IsNew)
        {
            await _bookService.AddBook(Input.ToBook(id, points));
        }
        else
        {
            await _bookService.EditBook(Input.ToBook(id, points));
        }

        return RedirectToPage("/Books/Index");
    }
}
