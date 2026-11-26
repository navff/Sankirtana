using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages;

[RESTAuthorize(PortalRoles.Administrator)]
[AutoValidateAntiforgeryToken(Order = 1100)]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class EditUser : PageModel
{
    private PortalUserService _portalUserService;
    
    // Если взяли пользователя на редактирование
    public PortalUser EditedUser { get; set; } = new();
    
    public EditUser(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public async Task<IActionResult> OnGet(string? id)
    {
        if (string.IsNullOrEmpty(id)) return Page();
        if (!ObjectId.TryParse(id, out _)) return BadRequest();

        var user = await _portalUserService.GetById(id);
        if (user == null) return NotFound();

        EditedUser = user;
        return Page();
    }
    
    public async Task<IActionResult> OnPost(PortalUserUpdateViewModel userViewModel)
    {
        if (!ObjectId.TryParse(userViewModel.Id, out var id) ||
            userViewModel.Role is not (PortalRoles.Administrator or PortalRoles.Seller or PortalRoles.Reader))
        {
            return BadRequest();
        }

        if (id == ObjectId.Empty)
        {
            await _portalUserService.AddUser(userViewModel.ToUser());
        }
        else
        {
            await _portalUserService.EditUser(userViewModel.ToUser());
        }
        
        return RedirectToPage("/Users/Index");
    }
}
