using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Ürün ID zorunludur")]
        [Display(Name = "Ürün ID")]
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Miktar zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den büyük olmalıdır")]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Birim fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır")]
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }
        
        [Required(ErrorMessage = "Toplam fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Toplam fiyat 0'dan büyük olmalıdır")]
        [Display(Name = "Toplam Fiyat")]
        public decimal TotalPrice { get; set; }
        
        [StringLength(50, ErrorMessage = "Durum en fazla 50 karakter olabilir")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Pending";
        
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        [Display(Name = "Ürün Adı")]
        public string ProductName { get; set; } = string.Empty;
        
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } = string.Empty;
    }
} 