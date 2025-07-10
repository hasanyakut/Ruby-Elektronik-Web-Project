using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace frontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string productApiUrl = "http://localhost:5144/products";

    private static List<ServiceRecord> _serviceRecords = new List<ServiceRecord>();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var response = await _httpClient.GetAsync(productApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(products ?? new List<Product>());
            }
            else
            {
                _logger.LogWarning($"ProductService returned status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "ProductService'e bağlanırken hata oluştu");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "ProductService isteği zaman aşımına uğradı");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler yüklenirken beklenmeyen hata oluştu");
        }
        
        return View(new List<Product>());
    }

    [HttpGet]
    public IActionResult ServisKayit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ServisKayit(ServiceRecord model)
    {
        // Custom validation for company name
        if (model.UserType == ServiceUserType.Corporate && string.IsNullOrWhiteSpace(model.FirmaAdi))
        {
            ModelState.AddModelError("FirmaAdi", "Kurumsal kullanıcılar için firma adı zorunludur");
        }

        if (ModelState.IsValid)
        {
            model.Id = _serviceRecords.Count + 1;
            _serviceRecords.Add(model);
            ViewBag.Basarili = true;
        }
        return View();
    }

    [HttpGet]
    public IActionResult ServisKayitlari()
    {
        return View(_serviceRecords);
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
