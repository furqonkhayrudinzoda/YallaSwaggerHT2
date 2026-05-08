using System;
namespace Domain.Models;

public class Order
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public DateOnly OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}