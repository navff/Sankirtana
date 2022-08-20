using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using Sankirtana.Web.Business.PortalUser;

namespace Sankirtana.Web.Pages;

public class EditUser : PageModel
{
    private PortalUserService _portalUserService;
    
    // Если взяли пользователя на редактирование
    public PortalUser User { get; set; }
    
    public EditUser(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public async Task OnGet(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            this.User = await _portalUserService.GetById(id);
        }
        else
        {
            this.User = new PortalUser();    
        }
    }
    
    public async Task<IActionResult> OnPost(PortalUserUpdateViewModel userViewModel)
    {
        if (userViewModel.Id == ObjectId.Empty.ToString())
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