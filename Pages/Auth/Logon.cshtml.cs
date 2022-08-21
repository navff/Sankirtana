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
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dbUser.Name),
            new Claim(ClaimTypes.Email, dbUser.Email),
            new Claim(ClaimTypes.Role, dbUser.Role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var user = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
        return RedirectToPage("/sales/AddSale");
    }
}