using System.Data;
using System.Security.Claims;

namespace Sankirtana.Web.Common.Helpers;

public static class UserHelper
{
    public static string GetId(this ClaimsPrincipal principal)
    {
       var idClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
       if (idClaim != null)
       {
           Console.WriteLine($"USER_ID: {idClaim.Value}");
           Console.WriteLine($"USER_NAME: {principal.Identity.Name}");
           return idClaim.Value;
       }
       
       throw new DataException("We found User without ID in Claims. " + principal.Identity.Name);
    }
}