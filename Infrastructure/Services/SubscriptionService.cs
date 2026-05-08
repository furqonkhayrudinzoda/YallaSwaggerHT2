using Dapper;
using Domain.Models;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly DataContext context = new DataContext();

    public async Task<List<Subscription>> GetSubscriptionsAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    company_id as CompanyId,
                    plan_type as PlanType,
                    meals_per_day as MealsPerDay,
                    price as Price,
                    start_date as StartDate,
                    end_date as EndDate,
                    is_active as IsActive,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from subscriptions";

        var result = await connection.QueryAsync<Subscription>(sql);
        return result.ToList();
    }

    public async Task<bool> AddSubscriptionAsync(Subscription subscription)
    {
        if (subscription.CompanyId <= 0)
        {
            Console.WriteLine("CompanyId is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(subscription.PlanType))
        {
            Console.WriteLine("PlanType is required");
            return false;
        }

        if (subscription.MealsPerDay <= 0)
        {
            Console.WriteLine("MealsPerDay must be greater than 0");
            return false;
        }

        if (subscription.Price <= 0)
        {
            Console.WriteLine("Price must be greater than 0");
            return false;
        }

        if (subscription.StartDate > subscription.EndDate)
        {
            Console.WriteLine("StartDate cannot be after EndDate");
            return false;
        }

        if (subscription.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (subscription.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        using var connection = context.GetConnection();
        connection.Open();

        const string check = "select count(*) from subscriptions where company_id = @CompanyId and plan_type = @PlanType";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { subscription.CompanyId, subscription.PlanType });

        if (exist > 0)
        {
            Console.WriteLine("Subscription already exists");
            return false;
        }

        var sql = @"insert into subscriptions(company_id, plan_type, meals_per_day, price, start_date, end_date, is_active, created_at, updated_at)
                    values(@CompanyId, @PlanType, @MealsPerDay, @Price, @StartDate, @EndDate, @IsActive, @CreatedAt, @UpdatedAt)";

        await connection.ExecuteAsync(sql, subscription);
        Console.WriteLine("Subscription successfully added");
        return true;
    }

    public async Task<bool> UpdateSubscriptionAsync(Subscription subscription)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (subscription.Id <= 0)
        {
            Console.WriteLine("Id is required");
            return false;
        }

        if (subscription.StartDate > subscription.EndDate)
        {
            Console.WriteLine("StartDate cannot be after EndDate");
            return false;
        }

        const string check = "select count(*) from subscriptions where id = @Id";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { subscription.Id });

        if (exist == 0)
        {
            Console.WriteLine("Subscription not found");
            return false;
        }

        var sql = @"update subscriptions
                    set company_id = @CompanyId,
                    plan_type = @PlanType,
                    meals_per_day = @MealsPerDay,
                    price = @Price,
                    start_date = @StartDate,
                    end_date = @EndDate,
                    is_active = @IsActive,
                    updated_at = @UpdatedAt
                    where id = @Id";

        await connection.ExecuteAsync(sql, subscription);
        Console.WriteLine("Subscription successfully updated");
        return true;
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"delete from subscriptions where id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });

        if (result == 0)
        {
            Console.WriteLine("Subscription not found");
            return false;
        }

        Console.WriteLine("Subscription successfully deleted");
        return true;
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    company_id as CompanyId,
                    plan_type as PlanType,
                    meals_per_day as MealsPerDay,
                    price as Price,
                    start_date as StartDate,
                    end_date as EndDate,
                    is_active as IsActive,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from subscriptions
                    where id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<Subscription>(sql, new { Id = id });
        if (result == null)
        {
            Console.WriteLine("Subscription not found");
            return null;
        }
        Console.WriteLine("Subscription found");
        return result;
    }
}