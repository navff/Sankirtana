using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.PortalUser;

namespace Sankirtana.Web.Pages.Users;

public class Index : PageModel
{
    public List<PortalUser> UsersList = new List<PortalUser>();
    private PortalUserService _portalUserService;

    public Index(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public async Task OnGet()
    {
        this.UsersList = await _portalUserService.GetUsers();
    }
}