using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2-100 karakter arasında olmalıdır")]
        [Display(Name = "Ürün Adı")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
    }
} 