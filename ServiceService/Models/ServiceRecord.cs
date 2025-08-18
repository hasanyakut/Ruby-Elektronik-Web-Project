using System.ComponentModel.DataAnnotations;

namespace ServiceService.Models;

public enum ServiceUserType
{
    Individual,
    Corporate
}

public class ServiceRecord
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Ad { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Soyad { get; set; } = string.Empty;
    
    [Required]
    public ServiceUserType UserType { get; set; }
    
    [StringLength(100)]
    public string? FirmaAdi { get; set; }
    
    [Required]
    [Phone]
    [StringLength(20)]
    public string TelefonNumarasi { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string UrunTuru { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string ArizaAciklamasi { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
