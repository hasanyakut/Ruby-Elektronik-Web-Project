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
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ad 2-100 karakter arasında olmalıdır")]
        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Kullanıcı tipi seçiniz")]
        [Display(Name = "Kullanıcı Tipi")]
        public UserType UserType { get; set; }
        
        [Display(Name = "Firma Adı")]
        public string? CompanyName { get; set; }
        
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }
    }
} 