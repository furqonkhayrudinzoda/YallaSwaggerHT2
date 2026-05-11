namespace Infrastructure.Dtos.GetCompanyWithOrderCount;

public class GetCompanyWithOrderCountDto
{
public int CompanyId { get; set; }   
public string CompanyName{get;set;} = "";
public int OrderCount { get; set; }
}
