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
            var query = _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt);

            var totalProducts = await query.CountAsync();
            var products = await query.Take(4).ToListAsync();

            ViewBag.TotalProducts = totalProducts;
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler yüklenirken hata oluştu");
            return View(new List<Product>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadMoreProducts(int skip = 0, int take = 4)
    {
        try
        {
            if (skip < 0) skip = 0;
            if (take <= 0 || take > 24) take = 4; // basic guardrails

            var products = await _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return PartialView("_ProductCards", products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoadMoreProducts hata verdi. skip={Skip} take={Take}", skip, take);
            return StatusCode(500, "Ürünler yüklenirken bir hata oluştu");
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
