using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Sales;

namespace Sankirtana.Web.Pages.Sales;

public class DeleteSale : PageModel
{
    private SalesService _salesService;

    public DeleteSale(SalesService salesService)
    {
        _salesService = salesService;
    }

    public void OnGet()
    {
        
    }
    
    public async Task OnPost(string id)
    {
        await _salesService.DeleteSale(id);
    }
}