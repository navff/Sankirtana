using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Business.Sales;

public class SaleUpdateViewModel
{
    public string Id { get; set; }
    
    public string ContactName { get; set; }
    
    public string ContactPhone { get; set; }

    public string BookId { get; set; }
}