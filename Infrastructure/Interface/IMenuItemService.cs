using System;
using Domain.Models;
namespace Infrastructure.Interface;

public interface IMenuItemService
{
    Task<List<MenuItem>> GetMenuItemsAsync();
    Task<bool> AddMenuItemAsync(MenuItem menuItem);
    Task<bool> UpdateMenuItemAsync(MenuItem menuItem);
    Task<bool> DeleteMenuItemAsync(int id);
    Task<MenuItem?> GetMenuItemByIdAsync(int id);
}