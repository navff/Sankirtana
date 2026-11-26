using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Sankirtana.Web.Business.Sales;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages.Reports;

[RESTAuthorize(Optional = true)]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class AllDay : PageModel
{
    private SalesService _salesService;
    public PeriodicStatistic Statistic;
    public DateTime DateStart;
    public DateTime DateEnd;

    public AllDay(SalesService salesService)
    {
        _salesService = salesService;
    }

    public async Task OnGet(DateTime? dateStart, DateTime? dateEnd)
    {
        dateStart ??= DateTime.Today;
        dateEnd ??= DateTime.Today;
            
        this.DateStart = dateStart.Value;
        this.DateEnd = dateEnd.Value.AddDays(1).AddSeconds(-1);
        this.Statistic = await _salesService.GetSalesStat(
            this.DateStart, 
            this.DateEnd);
    }
}
