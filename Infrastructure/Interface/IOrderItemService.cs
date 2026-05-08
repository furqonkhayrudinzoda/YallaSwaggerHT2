using System;
using Domain.Models;
namespace Infrastructure.Interface;
public interface IOrderItemService
{
    Task<List<OrderItem>> GetOrderItemsAsync();
    Task<bool> AddOrderItemAsync(OrderItem orderItem);
    Task<bool> UpdateOrderItemAsync(OrderItem orderItem);
    Task<bool> DeleteOrderItemAsync(int id);
    Task<OrderItem?> GetOrderItemByIdAsync(int id);
}