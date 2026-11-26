using MongoDB.Bson;

namespace Sankirtana.Web.Business.PortalUsers;

public class PortalUserUpdateViewModel
{
    public string Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Name => FirstName + " " + LastName;

    public string Token { get; set; }
    
    public string City { get; set; }
    
    public string Role { get; set; }
    
    public PortalUsers.PortalUser ToUser()
    {
        return new PortalUsers.PortalUser()
        {
            City = this.City,
            Id = string.IsNullOrEmpty(this.Id) 
                ? new ObjectId() 
                : new ObjectId(this.Id),
            Token = this.Token,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Role = this.Role
        };
    }
}
