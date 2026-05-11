using Dapper;
using Domain.Models;
using Infrastructure.Dtos.GetCompanyWithOrderCount;
using Infrastructure.Dtos.GetCompanyWithSubscriptionCount;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class CompanyService(DataContext context) : ICompanyService
{
    public async Task<List<Company>> GetCompaniesAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"Select id as Id, name as Name, address as Address, 
                    phone as Phone, email as Email, 
                    created_at as CreatedAt, updated_at as UpdatedAt
                    from companies";

        var result = await connection.QueryAsync<Company>(sql);
        return result.ToList();
    }

    public async Task<bool> AddCompanyAsync(Company company)
    {
        var sql = @"insert into companies(name, address, phone, email, created_at, updated_at)
                    values (@Name, @Address, @Phone, @Email, @CreatedAt, @UpdatedAt)";

        if (string.IsNullOrWhiteSpace(company.Name))
        {
            Console.WriteLine("Company name is required");
            return false;
        }

        if (company.Name.Length < 5 || company.Name.Length > 100)
        {
            Console.WriteLine("Company name length must be between 5 and 100 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Address))
        {
            Console.WriteLine("Address is required");
            return false;
        }

        if (company.Address.Length < 5 || company.Address.Length > 200)
        {
            Console.WriteLine("Address length must be between 5 and 200 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Phone))
        {
            Console.WriteLine("Phone is required");
            return false;
        }

        if (company.Phone.Length < 7 || company.Phone.Length > 20)
        {
            Console.WriteLine("Phone length must be between 7 and 20 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Email))
        {
            Console.WriteLine("Email is required");
            return false;
        }

        if (!company.Email.Contains("@") || !company.Email.Contains("."))
        {
            Console.WriteLine("Invalid email");
            return false;
        }

        if (company.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot  be in future");
            return false;
        }


        if (company.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot  be in future");
            return false;
        }


        using var connection = context.GetConnection();
        connection.Open();

        const string check = "Select count(*) from companies where Email = @Email";
        var exist = await connection.ExecuteScalarAsync<int>(check, new { Id = company.Id, Email = company.Email });
        if (exist > 0)
        {
            Console.WriteLine("Company already exists");
            return false;
        }

        await connection.ExecuteAsync(sql, company);
        Console.WriteLine("Company successfully added");
        return true;
    }

    public async Task<bool> UpdateCompanyAsync(Company company)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (string.IsNullOrWhiteSpace(company.Name))
        {
            Console.WriteLine("Company name is required");
            return false;
        }

        if (company.Name.Length < 5 || company.Name.Length > 100)
        {
            Console.WriteLine("Company name length must be between 5 and 100 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Address))
        {
            Console.WriteLine("Address is required");
            return false;
        }

        if (company.Address.Length < 5 || company.Address.Length > 200)
        {
            Console.WriteLine("Address length must be between 5 and 200 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Phone))
        {
            Console.WriteLine("Phone is required");
            return false;
        }

        if (company.Phone.Length < 7 || company.Phone.Length > 20)
        {
            Console.WriteLine("Phone length must be between 7 and 20 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(company.Email))
        {
            Console.WriteLine("Email is required");
            return false;
        }

        if (!company.Email.Contains("@") || !company.Email.Contains("."))
        {
            Console.WriteLine("Invalid email");
            return false;
        }

        if (company.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot  be in future");
            return false;
        }


        if (company.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot  be in future");
            return false;
        }

        const string check = "Select count(*) from companies where id = @Id";
        var exist = await connection.ExecuteScalarAsync<int>(check, new { Id = company.Id });
        if (exist == 0)
        {
            Console.WriteLine("Company not found");
            return false;
        }

        var sql = @"update companies
                    set name = @Name,
                    address = @Address,
                    phone = @Phone,
                    email = @Email,
                    updated_at = @UpdatedAt
                    where id = @Id";

        await connection.ExecuteAsync(sql, company);
        Console.WriteLine("Company successfully updated");
        return true;
    }

    public async Task<bool> DeleteCompanyAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"delete from companies where id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        if (result == 0)
        {
            Console.WriteLine("Company not found");
            return false;
        }

        Console.WriteLine("Company succesfully deleted");
        return true;
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"Select id as Id, name as Name, address as Address, 
                    phone as Phone, email as Email,
                    created_at as CreatedAt, updated_at as UpdatedAt
                    from companies where id = @Id";

        var company = await connection.QueryFirstOrDefaultAsync<Company>(sql, new { Id = id });
        if (company == null)
        {
            Console.WriteLine("Company not found");
            return null;
        }
        Console.WriteLine("Company found");
        return company;
    }

    public async Task<List<GetCompanyWithOrderCountDto>> GetCompaniesWithOrderCountAsync()
    {
        await using var connection = context.GetConnection();
        connection.Open();

        const string query = @"select c.id as CompanyId, c.name as CompanyName, count(o.id) as OrderCount
                            from companies c
                            left join orders o on c.id = o.company_id
                            group by c.id, c.name";
        var result = await connection.QueryAsync<GetCompanyWithOrderCountDto>(query);
        return result.ToList();
    }

    public async Task<List<GetCompanyWithSubscriptionCountDto>> GetCompaniesWithSubscriptionCountAsync()
    {
        await using var connection = context.GetConnection();
        connection.Open();

        const string query = @"select c.Id as CompanyId, c.Name as CompanyName, c.Address as CompanyAddress, count(s.Id) as SubscriptionCount
                        from companies c
                        join subscriptions s on c.Id = s.company_id
                        group by c.Id, c.Name, c.Address";
        var result = await connection.QueryAsync<GetCompanyWithSubscriptionCountDto>(query);
        return result.ToList();
    }
}