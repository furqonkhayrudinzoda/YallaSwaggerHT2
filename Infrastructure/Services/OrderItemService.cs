using Dapper;
using Domain.Models;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class OrderItemService(DataContext context) : IOrderItemService
{
    public async Task<List<OrderItem>> GetOrderItemsAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                order_id as OrderId,
                menu_item_id as MenuItemId,
                quantity as Quantity,
                price as Price,
                created_at as CreatedAt,
                updated_at as UpdatedAt
                from orderitems";
        var result = await connection.QueryAsync<OrderItem>(sql);
        return result.ToList();
    }
    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (orderItem.OrderId <= 0)
        {
            Console.WriteLine("OrderId is required");
            return false;
        }

        if (orderItem.MenuItemId <= 0)
        {
            Console.WriteLine("MenuItemId is required");
            return false;
        }

        if (orderItem.Quantity <= 0)
        {
            Console.WriteLine("Quantity must be greater than 0");
            return false;
        }

        if (orderItem.Price <= 0)
        {
            Console.WriteLine("Price must be greater than 0");
            return false;
        }

        if (orderItem.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (orderItem.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        const string check = "select count(*) from orderitems where order_id = @OrderId and menu_item_id = @MenuItemId";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { orderItem.OrderId, orderItem.MenuItemId });

        if (exist > 0)
        {
            Console.WriteLine("This item already exists in order");
            return false;
        }

        var sql = @"insert into orderitems(order_id, menu_item_id, quantity, price, created_at, updated_at)
                values(@OrderId, @MenuItemId, @Quantity, @Price, @CreatedAt, @UpdatedAt)";

        await connection.ExecuteAsync(sql, orderItem);
        Console.WriteLine("Order item successfully added");
        return true;
    }

    public async Task<bool> UpdateOrderItemAsync(OrderItem orderItem)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (orderItem.Id <= 0)
        {
            Console.WriteLine("Id is required");
            return false;
        }

        const string check = "select count(*) from orderitems where id = @Id";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { orderItem.Id });

        if (exist == 0)
        {
            Console.WriteLine("Order item not found");
            return false;
        }

        var sql = @"update orderitems
                    set order_id = @OrderId,
                    menu_item_id = @MenuItemId,
                    quantity = @Quantity,
                    price = @Price,
                    updated_at = @UpdatedAt
                    where id = @Id";
        await connection.ExecuteAsync(sql, orderItem);
        Console.WriteLine("Order item successfully updated");
        return true;
    }

    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"Delete from orderitems where id = @Id";

        var result = await connection.ExecuteAsync(sql, new { Id = id });

        if (result == 0)
        {
            Console.WriteLine("Order item not found");
            return false;
        }

        Console.WriteLine("Order item successfully deleted");
        return true;
    }

    public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    order_id as OrderId,
                    menu_item_id as MenuItemId,
                    quantity as Quantity,
                    price as Price,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from orderitems
                    where id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<OrderItem>(sql, new { Id = id });

        if (result == null)
        {
            Console.WriteLine("Order item not found");
            return null;
        }

        Console.WriteLine("Order item found");
        return result;
    }
}