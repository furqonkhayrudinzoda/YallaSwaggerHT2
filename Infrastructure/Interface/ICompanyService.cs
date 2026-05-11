using System;
using Domain.Models;
using Infrastructure.Dtos.GetCompanyWithOrderCount;
using Infrastructure.Dtos.GetCompanyWithSubscriptionCount;
namespace Infrastructure.Interface;

public interface ICompanyService
{
    Task<List<Company>> GetCompaniesAsync();
    Task<List<GetCompanyWithOrderCountDto>> GetCompaniesWithOrderCountAsync();
    Task<List<GetCompanyWithSubscriptionCountDto>> GetCompaniesWithSubscriptionCountAsync();
    Task<bool> AddCompanyAsync(Company company);
    Task<bool> UpdateCompanyAsync(Company company);
    Task<bool> DeleteCompanyAsync(int id);
    Task<Company?> GetCompanyByIdAsync(int id);
}

