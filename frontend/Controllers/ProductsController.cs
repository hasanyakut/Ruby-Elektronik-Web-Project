using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System;

namespace frontend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "http://localhost:5144/products";

        public ProductsController()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(products ?? new List<Product>());
                }
                else
                {
                    TempData["Error"] = $"Ürünler yüklenirken hata oluştu: {response.StatusCode}";
                    return View(new List<Product>());
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürünler yüklenirken bağlantı hatası oluştu";
                return View(new List<Product>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla eklendi";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Ürün eklenirken hata oluştu";
                    return View(product);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürün eklenirken bağlantı hatası oluştu";
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(product);
                }
                TempData["Error"] = "Ürün bulunamadı";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürün yüklenirken bağlantı hatası oluştu";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (!ModelState.IsValid) return View(product);
            
            try
            {
                product.Id = id;
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiUrl}/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla güncellendi";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Ürün güncellenirken hata oluştu";
                    return View(product);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürün güncellenirken bağlantı hatası oluştu";
                return View(product);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla silindi";
                }
                else
                {
                    TempData["Error"] = "Ürün silinirken hata oluştu";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürün silinirken bağlantı hatası oluştu";
            }
            
            return RedirectToAction("Index");
        }
    }
} 