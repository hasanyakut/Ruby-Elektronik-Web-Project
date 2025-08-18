using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties (for future use with related entities)
    public string ProductName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

