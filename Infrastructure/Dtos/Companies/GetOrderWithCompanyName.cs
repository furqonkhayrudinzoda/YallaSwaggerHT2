namespace Infrastructure.DTOS.Companies;

public class GetOrderWithCompanyName
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CompanyName { get; set; } = "";
}
