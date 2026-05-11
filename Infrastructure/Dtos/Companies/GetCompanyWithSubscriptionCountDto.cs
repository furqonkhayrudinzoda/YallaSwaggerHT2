namespace Infrastructure.Dtos.GetCompanyWithSubscriptionCount;
public class GetCompanyWithSubscriptionCountDto
{
public int CompanyId { get; set; }   
public string CompanyName{get;set;} = "";
public int SubscriptionCount { get; set; }
}
