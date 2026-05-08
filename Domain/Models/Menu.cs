using System;
namespace Domain.Models;
public class Menu
{
    public int Id { get; set; }
    public DateOnly MenuDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
