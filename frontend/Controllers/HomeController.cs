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
    private readonly string serviceApiUrl = "http://localhost:7004/servicerecords";

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
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(serviceApiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Basarili = true;
                }
                else
                {
                    _logger.LogError($"ServiceService returned status code: {response.StatusCode}");
                    ModelState.AddModelError("", "Servis kaydı oluşturulurken bir hata oluştu");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ServiceService'e bağlanırken hata oluştu");
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
            var response = await _httpClient.GetAsync(serviceApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var serviceRecords = JsonSerializer.Deserialize<List<ServiceRecord>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(serviceRecords ?? new List<ServiceRecord>());
            }
            else
            {
                _logger.LogWarning($"ServiceService returned status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "ServiceService'e bağlanırken hata oluştu");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "ServiceService isteği zaman aşımına uğradı");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Servis kayıtları yüklenirken beklenmeyen hata oluştu");
        }
        
        return View(new List<ServiceRecord>());
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
