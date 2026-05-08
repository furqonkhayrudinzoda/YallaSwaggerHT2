using Dapper;
using Domain.Models;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class MenuItemService : IMenuItemService
{
    private readonly DataContext context = new DataContext();

    public async Task<List<MenuItem>> GetMenuItemsAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    menu_id as MenuId,
                    name as Name,
                    description as Description,
                    price as Price,
                    category as Category,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from menuitems";

        var result = await connection.QueryAsync<MenuItem>(sql);
        return result.ToList();
    }

    public async Task<bool> AddMenuItemAsync(MenuItem menuItem)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (menuItem.MenuId <= 0)
        {
            Console.WriteLine("MenuId is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(menuItem.Name))
        {
            Console.WriteLine("Name is required");
            return false;
        }

        if (menuItem.Name.Length < 3 || menuItem.Name.Length > 100)
        {
            Console.WriteLine("Name length must be between 3 and 100");
            return false;
        }

        if (menuItem.Price <= 0)
        {
            Console.WriteLine("Price must be greater than 0");
            return false;
        }

        if (string.IsNullOrWhiteSpace(menuItem.Category))
        {
            Console.WriteLine("Category is required");
            return false;
        }

        if (menuItem.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (menuItem.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        const string check = "select count(*) from menuitems where menu_id = @MenuId and name = @Name";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { menuItem.MenuId, menuItem.Name });

        if (exist > 0)
        {
            Console.WriteLine("Menu item already exists");
            return false;
        }

        var sql = @"insert into menuitems(menu_id, name, description, price, category, created_at, updated_at)
                    values(@MenuId, @Name, @Description, @Price, @Category, @CreatedAt, @UpdatedAt)";

        await connection.ExecuteAsync(sql, menuItem);
        Console.WriteLine("Menu item successfully added");
        return true;
    }

    public async Task<bool> UpdateMenuItemAsync(MenuItem menuItem)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (menuItem.Id <= 0)
        {
            Console.WriteLine("Id is required");
            return false;
        }

        if (menuItem.MenuId <= 0)
        {
            Console.WriteLine("MenuId is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(menuItem.Name))
        {
            Console.WriteLine("Name is required");
            return false;
        }

        if (menuItem.Price <= 0)
        {
            Console.WriteLine("Price must be greater than 0");
            return false;
        }

        const string check = "select count(*) from menuitems where id = @Id";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { menuItem.Id });

        if (exist == 0)
        {
            Console.WriteLine("Menu item not found");
            return false;
        }

        var sql = @"update menuitems
                    set menu_id = @MenuId,
                    name = @Name,
                    description = @Description,
                    price = @Price,
                    category = @Category,
                    updated_at = @UpdatedAt
                    where id = @Id";

        await connection.ExecuteAsync(sql, menuItem);
        Console.WriteLine("Menu item successfully updated");
        return true;
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();

        var sql = @"delete from menuitems where id = @Id";

        var result = await connection.ExecuteAsync(sql, new { Id = id });

        if (result == 0)
        {
            Console.WriteLine("Menu item not found");
            return false;
        }

        Console.WriteLine("Menu item successfully deleted");

        return true;
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    menu_id as MenuId,
                    name as Name,
                    description as Description,
                    price as Price,
                    category as Category,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from menuitems
                    where id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<MenuItem>(sql, new { Id = id });

        if (result == null)
        {
            Console.WriteLine("Menu item not found");
            return null;
        }

        Console.WriteLine("Menu item found");

        return result;
    }
}