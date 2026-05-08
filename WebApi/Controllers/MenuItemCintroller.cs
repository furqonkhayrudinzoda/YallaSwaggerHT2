using Domain.Models;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/menu_items")]
public class MenuItemController(IMenuItemService menuItemService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<MenuItem>> GetListAsync()
    {
        return await menuItemService1.GetMenuItemsAsync();
    }

    [HttpPost]
    public async Task<bool> AddMenuItemAsync(MenuItem menuItem)
    {
        return await menuItemService1.AddMenuItemAsync(menuItem);
    }

    [HttpPut]
    public async Task<bool> UpdateMenuItemAsync(MenuItem menuItem)
    {
        return await menuItemService1.UpdateMenuItemAsync(menuItem);
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        return await menuItemService1.DeleteMenuItemAsync(id);
    }

    [HttpGet("{id:int}")]
    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        return await menuItemService1.GetMenuItemByIdAsync(id);
    }
}