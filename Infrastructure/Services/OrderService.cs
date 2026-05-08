using Dapper;
using Domain.Models;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly DataContext context = new DataContext();

    public async Task<List<Order>> GetOrdersAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    company_id as CompanyId,
                    order_date as OrderDate,
                    status as Status,
                    total_amount as TotalAmount,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from orders";

        var result = await connection.QueryAsync<Order>(sql);

        return result.ToList();
    }

    public async Task<bool> AddOrderAsync(Order order)
    {
        if (order.CompanyId <= 0)
        {
            Console.WriteLine("CompanyId is required");
            return false;
        }

        if (order.OrderDate == default)
        {
            Console.WriteLine("OrderDate is required");
            return false;
        }

        if (order.OrderDate > DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("OrderDate cannot be in future");
            return false;
        }

        if (string.IsNullOrWhiteSpace(order.Status))
        {
            Console.WriteLine("Status is required");
            return false;
        }

        if (order.TotalAmount <= 0)
        {
            Console.WriteLine("TotalAmount must be greater than 0");
            return false;
        }

        if (order.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (order.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        using var connection = context.GetConnection();
        connection.Open();

        const string check =
            "select count(*) from orders where company_id = @CompanyId and order_date = @OrderDate";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { order.CompanyId, order.OrderDate });

        if (exist > 0)
        {
            Console.WriteLine("Order already exists for this date");
            return false;
        }

        var sql = @"insert into orders(company_id, order_date, status, total_amount, created_at, updated_at)
                    values(@CompanyId, @OrderDate, @Status, @TotalAmount, @CreatedAt, @UpdatedAt)";

        await connection.ExecuteAsync(sql, order);
        Console.WriteLine("Order successfully added");
        return true;
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (order.CompanyId <= 0)
        {
            Console.WriteLine("CompanyId is required");
            return false;
        }

        if (order.OrderDate == default)
        {
            Console.WriteLine("OrderDate is required");
            return false;
        }

        if (order.OrderDate > DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("OrderDate cannot be in future");
            return false;
        }

        if (string.IsNullOrWhiteSpace(order.Status))
        {
            Console.WriteLine("Status is required");
            return false;
        }

        if (order.TotalAmount <= 0)
        {
            Console.WriteLine("TotalAmount must be greater than 0");
            return false;
        }

        if (order.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (order.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        const string check = "select count(*) from orders where id = @Id";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { order.Id });

        if (exist == 0)
        {
            Console.WriteLine("Order not found");
            return false;
        }

        var sql = @"update orders
                    set company_id = @CompanyId,
                    order_date = @OrderDate,
                    status = @Status,
                    total_amount = @TotalAmount,
                    updated_at = @UpdatedAt
                    where id = @Id";

        await connection.ExecuteAsync(sql, order);
        Console.WriteLine("Order successfully updated");
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"delete from orders where id = @Id";

        var result = await connection.ExecuteAsync(sql, new { Id = id });

        if (result == 0)
        {
            Console.WriteLine("Order not found");
            return false;
        }

        Console.WriteLine("Order successfully deleted");
        return true;
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    company_id as CompanyId,
                    order_date as OrderDate,
                    status as Status,
                    total_amount as TotalAmount,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from orders
                    where id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
        if (result == null)
        {
            Console.WriteLine("Order not found");
            return null;
        }
        Console.WriteLine("Order found");
        return result;
    }
}