using Domain.Models;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/menus")]
public class MenuController(IMenuService menuService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<Menu>> GetListAsync()
    {
        return await menuService1.GetMenusAsync();
    }

    [HttpPost]
    public async Task<bool> AddMenuAsync(Menu menu)
    {
        return await menuService1.AddMenuAsync(menu);
    }

    [HttpPut]
    public async Task<bool> UpdateMenuAsync(Menu menu)
    {
        return await menuService1.UpdateMenuAsync(menu);
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> DeleteMenuAsync(int id)
    {
        return await menuService1.DeleteMenuAsync(id);
    }

    [HttpGet("{id:int}")]
    public async Task<Menu?> GetMenuByIdAsync(int id)
    {
        return await menuService1.GetMenuByIdAsync(id);
    }
}