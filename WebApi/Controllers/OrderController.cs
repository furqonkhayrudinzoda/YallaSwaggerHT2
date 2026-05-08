using Domain.Models;
using Infrastructure.DTOS.Companies;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<Order>> GetListAsync()
    {
        return await orderService1.GetOrdersAsync();
    }
    
    [HttpGet ("with-company-names")]
    public async Task<List<GetOrderWithCompanyName>> GetAllOrdersWithCompanyNamesAsync()
    {
        return await orderService1.GetAllOrdersWithCompanyNamesAsync();
    }

    [HttpPost]
    public async Task<bool> AddOrderAsync(Order order)
    {
        return await orderService1.AddOrderAsync(order);
    }

    [HttpPut]
    public async Task<bool> UpdateOrderAsync(Order order)
    {
        return await orderService1.UpdateOrderAsync(order);
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> DeleteOrderAsync(int id)
    {
        return await orderService1.DeleteOrderAsync(id);
    }

    [HttpGet("{id:int}")]
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await orderService1.GetOrderByIdAsync(id);
    }
}