using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Sales;

namespace Sankirtana.Web.Pages.December;

public class Index : PageModel
{
    private SalesService _salesService;
    public PeriodicStatistic Statistic;


    public Index(SalesService salesService)
    {
        _salesService = salesService;
    }

    public async Task OnGet()
    {
        var dateStart = new DateTime(2024, 12, 1);
        var dateEnd = new DateTime(2024, 12, 31, 23, 59, 59);
            
        this.Statistic = await _salesService.GetSalesStat(
            dateStart, 
            dateEnd);         
    }
}