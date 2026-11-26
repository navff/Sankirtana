using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RESTAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;
    public bool Optional { get; set; }

    public RESTAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Cookies["Token"];
        PortalUser? user = null;
        if (!string.IsNullOrWhiteSpace(token))
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<PortalUserService>();
            user = await userService.GetByToken(token);
        }

        if (user == null)
        {
            if (!Optional || _roles.Length > 0)
            {
                context.Result = new ForbidResult();
            }
            return;
        }

        var role = user.EffectiveRole;
        if (_roles.Length > 0 && !_roles.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
            return;
        }

        var identity = new Identity { Name = user.Name, IsAuthenticated = true, Id = user.Id.ToString() };
        var roles = string.IsNullOrWhiteSpace(role) ? Array.Empty<string>() : new[] { role };
        context.HttpContext.User = new GenericPrincipal(identity, roles);
    }
}
