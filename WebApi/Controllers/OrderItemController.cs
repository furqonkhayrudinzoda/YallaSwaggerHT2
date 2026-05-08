using Domain.Models;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/order_items")]
public class OrderItemController(IOrderItemService orderItemService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<OrderItem>> GetListAsync()
    {
        return await orderItemService1.GetOrderItemsAsync();
    }

    [HttpPost]
    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
    {
        return await orderItemService1.AddOrderItemAsync(orderItem);
    }

    [HttpPut]
    public async Task<bool> UpdateOrderItemAsync(OrderItem orderItem)
    {
        return await orderItemService1.UpdateOrderItemAsync(orderItem);
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> DeleteOrderItemAsync(int id)
    {
        return await orderItemService1.DeleteOrderItemAsync(id);
    }

    [HttpGet("{id:int}")]
    public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
    {
        return await orderItemService1.GetOrderItemByIdAsync(id);
    }
}