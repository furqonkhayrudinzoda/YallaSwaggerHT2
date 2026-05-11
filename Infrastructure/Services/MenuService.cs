using Dapper;
using Domain.Models;
using Infrastructure.Interface;

namespace Infrastructure.Services;

public class MenuService(DataContext context) : IMenuService
{
    public async Task<List<Menu>> GetMenusAsync()
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    menu_date as MenuDate,
                    is_active as IsActive,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from menus";
        var result = await connection.QueryAsync<Menu>(sql);
        return result.ToList();
    }

    public async Task<bool> AddMenuAsync(Menu menu)
    {
        var sql = @"insert into menus(menu_date, is_active, created_at, updated_at)
                    values(@MenuDate, @IsActive, @CreatedAt, @UpdatedAt)";

        if (menu.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (menu.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        using var connection = context.GetConnection();
        connection.Open();

        const string check = "select count(*) from menus where menu_date = @MenuDate";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { menu.MenuDate });

        if (exist > 0)
        {
            Console.WriteLine("Menu already exists");
            return false;
        }

        await connection.ExecuteAsync(sql, menu);

        Console.WriteLine("Menu successfully added");

        return true;
    }

    public async Task<bool> UpdateMenuAsync(Menu menu)
    {
        using var connection = context.GetConnection();
        connection.Open();

        if (menu.CreatedAt > DateTime.Now)
        {
            Console.WriteLine("CreatedAt cannot be in future");
            return false;
        }

        if (menu.UpdatedAt > DateTime.Now)
        {
            Console.WriteLine("UpdatedAt cannot be in future");
            return false;
        }

        const string check = "select count(*) from menus where id = @Id";

        var exist = await connection.ExecuteScalarAsync<int>(check, new { menu.Id });

        if (exist == 0)
        {
            Console.WriteLine("Menu not found");
            return false;
        }

        var sql = @"update menus
                    set menu_date = @MenuDate,
                    is_active = @IsActive,
                    updated_at = @UpdatedAt
                    where id = @Id";

        await connection.ExecuteAsync(sql, menu);
        Console.WriteLine("Menu successfully updated");
        return true;
    }

    public async Task<bool> DeleteMenuAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"delete from menus where id = @Id";

        var result = await connection.ExecuteAsync(sql, new { Id = id });

        if (result == 0)
        {
            Console.WriteLine("Menu not found");
            return false;
        }

        Console.WriteLine("Menu successfully deleted");

        return true;
    }

    public async Task<Menu?> GetMenuByIdAsync(int id)
    {
        using var connection = context.GetConnection();
        connection.Open();
        var sql = @"select id as Id,
                    menu_date as MenuDate,
                    is_active as IsActive,
                    created_at as CreatedAt,
                    updated_at as UpdatedAt
                    from menus
                    where id = @Id";

        var menu = await connection.QueryFirstOrDefaultAsync<Menu>(sql, new { Id = id });

        if (menu == null)
        {
            Console.WriteLine("Menu not found");
            return null;
        }

        Console.WriteLine("Menu found");
        return menu;
    }
}