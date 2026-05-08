using Domain.Models;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController(ICompanyService companyService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<Company>> GetListAsync()
    {
        return await companyService1.GetCompaniesAsync();
    }

    [HttpPost]
    public async Task<bool> AddCompanyAsync(Company company)
    {
        return await companyService1.AddCompanyAsync(company);
    }

    [HttpPut]
    public async Task<bool> UpdateCompanyAsync(Company company)
    {
        return await companyService1.UpdateCompanyAsync(company);
    }

    [HttpDelete ("{id:int}")]
    public async Task<bool> DeleteCompanyAsync(int id)
    {
        return await companyService1.DeleteCompanyAsync(id);
    }

    [HttpGet ("{id:int}")]
    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await companyService1.GetCompanyByIdAsync(id);
    }
}