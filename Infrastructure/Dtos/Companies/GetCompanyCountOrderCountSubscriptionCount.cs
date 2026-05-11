namespace Infrastructure.DTOS.Companies;

public class GetCompanyCountOrderCountSubscriptionCount
{
    public int CompanyId { get; set; }
    public int OrderCount { get; set; }
    public int SubscriptionCount { get; set; }
}
