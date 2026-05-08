using System;
using Domain.Models;
namespace Infrastructure.Interface;
public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<bool> AddOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int id);
    Task<Order?> GetOrderByIdAsync(int id);
}