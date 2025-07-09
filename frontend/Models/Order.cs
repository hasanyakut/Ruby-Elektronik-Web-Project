using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
} 