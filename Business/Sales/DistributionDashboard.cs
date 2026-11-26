using Sankirtana.Web.Business.Books;

namespace Sankirtana.Web.Business.Sales;

public class DistributionDashboard
{
    private static readonly (string Code, string Name)[] BookCategories =
    {
        (BookCategory.MahaBig, "Маха-биг"),
        (BookCategory.Big, "Биг"),
        (BookCategory.Medium, "Средние"),
        (BookCategory.Small, "Маленькие")
    };

    public long TotalBooks { get; }
    public decimal VolumePoints { get; }
    public int Participants { get; }
    public IReadOnlyList<long> DailyBooks { get; }
    public IReadOnlyList<BookTotal> Categories { get; }
    public IReadOnlyList<BookTotal> TopBooks { get; }

    public DistributionDashboard(DateTime month, IReadOnlyCollection<Sale> sales)
    {
        TotalBooks = sales.Sum(sale => (long)sale.Count);
        VolumePoints = sales.Sum(sale => (sale.Book?.VolumePoints ?? 0) * sale.Count);
        Participants = sales.Where(sale => sale.User is not null)
            .DistinctBy(sale => sale.User.Id).Count();

        var dailyBooks = new long[DateTime.DaysInMonth(month.Year, month.Month)];
        foreach (var sale in sales)
        {
            dailyBooks[sale.Date.Day - 1] += sale.Count;
        }
        DailyBooks = dailyBooks;

        var categories = BookCategories.Select(category => new BookTotal(category.Name,
            sales.Where(sale => sale.Book?.Category == category.Code).Sum(sale => (long)sale.Count))).ToList();
        var otherBooks = TotalBooks - categories.Sum(category => category.Count);
        if (otherBooks != 0)
        {
            categories.Add(new BookTotal("Другие", otherBooks));
        }
        Categories = categories;

        TopBooks = sales.Where(sale => !string.IsNullOrWhiteSpace(sale.Book?.Name))
            .GroupBy(sale => sale.Book.Id)
            .Select(group => new BookTotal(group.MaxBy(sale => sale.Date)!.Book.Name,
                group.Sum(sale => (long)sale.Count)))
            .Where(book => book.Count > 0)
            .OrderByDescending(book => book.Count)
            .ThenBy(book => book.Name)
            .Take(5)
            .ToList();
    }

    public record BookTotal(string Name, long Count);
}
