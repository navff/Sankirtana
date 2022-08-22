using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages.Users;

[RESTAuthorize("Admin")]
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