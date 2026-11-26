using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Sales;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages.Dashboard;

[RESTAuthorize]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class Index : PageModel
{
    public static readonly CultureInfo DisplayCulture = CultureInfo.GetCultureInfo("ru-RU");
    private static readonly TimeZoneInfo ReportingTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
    private readonly SalesService _salesService;

    public Index(SalesService salesService)
    {
        _salesService = salesService;
    }

    public DateTime Month { get; private set; }
    public string MonthLabel => DisplayCulture.TextInfo.ToTitleCase(Month.ToString("MMMM yyyy", DisplayCulture));
    public string MonthValue => Month.ToString("yyyy-MM", CultureInfo.InvariantCulture);
    public int ElapsedDays { get; private set; }
    public int? Goal { get; private set; }
    public decimal GoalPercent => Goal is int goal ? 100m * Statistic.TotalBooks / goal : 0m;
    public long Remaining => Goal is int goal ? Math.Max(0, goal - Statistic.TotalBooks) : 0;
    public DistributionDashboard Statistic { get; private set; } = null!;

    public async Task OnGetAsync(string? month, CancellationToken cancellationToken)
    {
        var today = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, ReportingTimeZone).Date;
        var currentMonth = new DateTime(today.Year, today.Month, 1);
        Month = DateTime.TryParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var selectedMonth) && selectedMonth.Year is > 1 and < 9999
            ? selectedMonth
            : currentMonth;

        if (int.TryParse(Environment.GetEnvironmentVariable("MONTHLY_BOOK_GOAL"), NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var goal) && goal > 0)
        {
            Goal = goal;
        }

        var sales = await _salesService.GetSalesForMonth(Month, cancellationToken);
        Statistic = new DistributionDashboard(Month, sales);
        ElapsedDays = Math.Clamp((today - Month).Days + 1, 0, Statistic.DailyBooks.Count);
        if (sales.Count > 0)
        {
            ElapsedDays = Math.Max(ElapsedDays, sales.Max(sale => sale.Date.Day));
        }
    }

    public string AdjacentMonth(int offset) => Month.AddMonths(offset).ToString("yyyy-MM", CultureInfo.InvariantCulture);
}
