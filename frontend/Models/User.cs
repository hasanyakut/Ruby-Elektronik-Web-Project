using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public enum UserType
    {
        Individual,
        Corporate
    }

    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Ad 2-200 karakter arasında olmalıdır")]
        [Display(Name = "Ad Soyad")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kullanıcı tipi seçiniz")]
        [Display(Name = "Kullanıcı Tipi")]
        public UserType UserType { get; set; }
        
        [StringLength(200, ErrorMessage = "Firma adı en fazla 200 karakter olabilir")]
        [Display(Name = "Firma Adı")]
        public string? CompanyName { get; set; }
        
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        [Display(Name = "Adres")]
        public string? Address { get; set; }
        
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }
} 