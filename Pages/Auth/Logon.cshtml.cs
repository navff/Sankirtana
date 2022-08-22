using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Pages.Auth;

public class Logon : PageModel
{
    private PortalUserService _portalUserService;

    public Logon(PortalUserService portalUserService)
    {
        _portalUserService = portalUserService;
    }

    public async Task<IActionResult> OnGet(string token)
    {
        var dbUser = await _portalUserService.GetByToken(token);
        if (dbUser == null)
        {
            return RedirectToPage("Index");
        }
        
        Response.Cookies.Append(
            "Token", 
            token, 
            new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(365)
            });

        return RedirectToPage("/sales/AddSale");
    }
}