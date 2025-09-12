using System.ComponentModel.DataAnnotations;

namespace RubyElektronik.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Ürün adı 2-200 karakter arasında olmalıdır")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        
        [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir")]
        [Display(Name = "Kategori")]
        public string? Category { get; set; }
        
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }
} 