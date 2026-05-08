using System;
using Domain.Models;
namespace Infrastructure.Interface;
public interface IMenuService
{    Task<List<Menu>> GetMenusAsync();
    Task<bool> AddMenuAsync(Menu menu);
    Task<bool> UpdateMenuAsync(Menu menu);
    Task<bool> DeleteMenuAsync(int id);
    Task<Menu?> GetMenuByIdAsync(int id);
}