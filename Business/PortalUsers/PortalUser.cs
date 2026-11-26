using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sankirtana.Web.Business.PortalUsers;

[BsonIgnoreExtraElements]
public class PortalUser
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [Display(Name = "Имя")]
    public string FirstName { get; set; }
    
    [Display(Name = "Фамилия")]
    public string LastName { get; set; }

    [Display(Name = "Имя, фамилия")]
    public string Name => FirstName + " " + LastName;

    [Display(Name = "Токен для аутентификации")]
    public string Token { get; set; }
    
    [Display(Name = "Город")]
    public string City { get; set; }
    
    public string Role { get; set; }

    [BsonIgnoreIfDefault]
    public bool RoleAssignedByAdministrator { get; set; }

    [BsonIgnore]
    public string? EffectiveRole =>
        !RoleAssignedByAdministrator && string.Equals(Role, PortalRoles.Administrator, StringComparison.OrdinalIgnoreCase)
            ? PortalRoles.Seller
            : Role;
}

public class PortalUserShort
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [Display(Name = "Имя, фамилия")]
    public string Name { get; set; }

    [Display(Name = "Город")]
    public string City { get; set; }
    
    public string Role { get; set; }
}
