using Domain.Models;
using Infrastructure.Interface;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionController(ISubscriptionService subscriptionService1) : ControllerBase
{
    [HttpGet]
    public async Task<List<Subscription>> GetListAsync()
    {
        return await subscriptionService1.GetSubscriptionsAsync();
    }

    [HttpPost]
    public async Task<bool> AddSubscriptionAsync(Subscription subscription)
    {
        return await subscriptionService1.AddSubscriptionAsync(subscription);
    }

    [HttpPut]
    public async Task<bool> UpdateSubscriptionAsync(Subscription subscription)
    {
        return await subscriptionService1.UpdateSubscriptionAsync(subscription);
    }

    [HttpDelete("{id:int}")]
    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        return await subscriptionService1.DeleteSubscriptionAsync(id);
    }

    [HttpGet("{id:int}")]
    public async Task<Subscription?> GetSubscriptionByIdAsync(int id)
    {
        return await subscriptionService1.GetSubscriptionByIdAsync(id);
    }
}