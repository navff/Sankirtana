using System.Security.Principal;

namespace Sankirtana.Web.Common;

public class Identity : IIdentity
{
    public string Name { get; set; }
    
    public string Id { get; set; }
    
    public string AuthenticationType { get; set; }
    public bool IsAuthenticated { get; set; }
}