using System;
using System.ComponentModel.DataAnnotations;

namespace RubyElektronik.Models
{
    public enum ServiceUserType
    {
        Individual,
        Corporate
    }

    public class ServiceRecord
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır")]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kullanıcı tipi seçiniz")]
        [Display(Name = "Kullanıcı Tipi")]
        public ServiceUserType UserType { get; set; }
        
        [Display(Name = "Firma Adı")]
        public string? FirmaAdi { get; set; }
        
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        public string TelefonNumarasi { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ürün türü zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün türü 2-100 karakter arasında olmalıdır")]
        [Display(Name = "Ürün Türü")]
        public string UrunTuru { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Arıza açıklaması zorunludur")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Arıza açıklaması en az 10 karakter olmalıdır")]
        [Display(Name = "Arıza Açıklaması")]
        public string ArizaAciklamasi { get; set; } = string.Empty;
        
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }
} 