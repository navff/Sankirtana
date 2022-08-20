using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Book;
using Sankirtana.Web.Business.PortalUser;

namespace Sankirtana.Web.Pages;

public class DeleteUser : PageModel
{
    private readonly PortalUserService _portalUserService;

    public DeleteUser(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public void OnGet()
    {
        
    }

    public async Task OnPost(string id)
    {
        await _portalUserService.DeleteUser(id);
    }
}