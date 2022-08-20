using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sankirtana.Web.Business.PortalUsers;

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

    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Display(Name = "Номер телефона")]
    public string Phone { get; set; }
    
    [Display(Name = "Токен для аутентификации")]
    public string Token { get; set; }
    
    [Display(Name = "Город")]
    public string City { get; set; }
}

public class PortaUserShort
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    
    [Display(Name = "Имя, фамилия")]
    public string Name { get; set; }

    [Display(Name = "Город")]
    public string City { get; set; }
}