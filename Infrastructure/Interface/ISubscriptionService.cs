using System;
using Domain.Models;
using Infrastructure.DTOS.Companies;
namespace Infrastructure.Interface;

public interface ISubscriptionService
{
    Task<List<Subscription>> GetSubscriptionsAsync();
    Task<List<GetCompanyCountOrderCountSubscriptionCount>> GetCompanyCountOrderCountSubscriptionCountAsync();
    Task<bool> AddSubscriptionAsync(Subscription subscription);
    Task<bool> UpdateSubscriptionAsync(Subscription subscription);
    Task<bool> DeleteSubscriptionAsync(int id);
    Task<Subscription?> GetSubscriptionByIdAsync(int id);
}
