using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubyElektronik.Models;
using RubyElektronik.Data;

namespace RubyElektronik.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await _context.Products.Where(p => p.IsActive).ToListAsync();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler yüklenirken hata oluştu");
            return View(new List<Product>());
        }
    }

    [HttpGet]
    public IActionResult ServisKayit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ServisKayit(ServiceRecord model)
    {
        // Custom validation for company name
        if (model.UserType == ServiceUserType.Corporate && string.IsNullOrWhiteSpace(model.FirmaAdi))
        {
            ModelState.AddModelError("FirmaAdi", "Kurumsal kullanıcılar için firma adı zorunludur");
        }

        if (ModelState.IsValid)
        {
            try
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.ServiceRecords.Add(model);
                await _context.SaveChangesAsync();
                ViewBag.Basarili = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Servis kaydı oluşturulurken hata oluştu");
                ModelState.AddModelError("", "Servis kaydı oluşturulurken bir hata oluştu");
            }
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ServisKayitlari()
    {
        try
        {
            var serviceRecords = await _context.ServiceRecords.Where(s => s.IsActive).ToListAsync();
            return View(serviceRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Servis kayıtları yüklenirken hata oluştu");
            return View(new List<ServiceRecord>());
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
