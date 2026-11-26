using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.PortalUsers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages;

[RESTAuthorize(PortalRoles.Administrator)]
[AutoValidateAntiforgeryToken(Order = 1100)]
public class DeleteUser : PageModel
{
    private readonly PortalUserService _portalUserService;

    public DeleteUser(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public IActionResult OnGet() => RedirectToPage("/Users/Index");

    public async Task<IActionResult> OnPost(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return BadRequest();
        await _portalUserService.DeleteUser(id);
        return RedirectToPage("/Users/Index");
    }
}
