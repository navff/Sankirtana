using Sankirtana.Web.Business.PortalUsers;

namespace Sankirtana.Web.Business.Sales;

public class UserSalesStatisticRecord
{
    public PortalUserShort User { get; set; }
    public List<BookSaleStatisticRecord> Sales { get; set; }
}

public class BookSaleStatisticRecord
{
    public string SaleId { get; set; }
    public string BookName { get; set; }
    public int Quantity { get; set; }
    public decimal VolumePoints { get; set; }
    public PortalUserShort User { get; set; }
    
}

public class PeriodicStatistic
{
    public List<UserSalesStatisticRecord> Records { get; set; }
    public int TotalBookCount { get; set; }
    public int MahaBig { get; set; }
    public int Big { get; set; }
    public int Medium { get; set; }
    public int Small { get; set; }
    public decimal VolumePoints { get; set; }
}