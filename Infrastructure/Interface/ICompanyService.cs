using System;
using Domain.Models;
namespace Infrastructure.Interface;

public interface ICompanyService
{
    Task<List<Company>> GetCompaniesAsync();
    Task<bool> AddCompanyAsync(Company company);
    Task<bool> UpdateCompanyAsync(Company company);
    Task<bool> DeleteCompanyAsync(int id);
    Task<Company?> GetCompanyByIdAsync(int id);
}

