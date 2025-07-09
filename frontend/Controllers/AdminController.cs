using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace frontend.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string productApiUrl = "http://localhost:5144/products";
        private readonly string orderApiUrl = "http://localhost:5145/orders";
        private readonly string userApiUrl = "http://localhost:5153/users";

        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        // Login sayfası
        [Route("")]
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            // Eğer zaten giriş yapmışsa dashboard'a yönlendir
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
            {
                return RedirectToAction("Dashboard");
            }
            
            ViewData["Title"] = "Ruby Elektronik - Admin Giriş";
            return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "12345")
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                HttpContext.Session.SetString("AdminUsername", username);
                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }
        }

        // Logout
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Admin Dashboard
        [Route("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Admin Dashboard";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
            try
            {
                // Get counts for dashboard
                var productResponse = await _httpClient.GetAsync(productApiUrl);
                var orderResponse = await _httpClient.GetAsync(orderApiUrl);
                var userResponse = await _httpClient.GetAsync(userApiUrl);

                var productCount = 0;
                var orderCount = 0;
                var userCount = 0;

                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<Product>>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    productCount = products?.Count ?? 0;
                }

                if (orderResponse.IsSuccessStatusCode)
                {
                    var orderJson = await orderResponse.Content.ReadAsStringAsync();
                    var orders = JsonSerializer.Deserialize<List<Order>>(orderJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    orderCount = orders?.Count ?? 0;
                }

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<User>>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    userCount = users?.Count ?? 0;
                }

                ViewBag.ProductCount = productCount;
                ViewBag.OrderCount = orderCount;
                ViewBag.UserCount = userCount;

                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "Dashboard verileri yüklenirken hata oluştu";
                return View();
            }
        }

        // Products Management
        [Route("products")]
        public async Task<IActionResult> Products()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Ürün Yönetimi";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
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

        [Route("products/create")]
        [HttpGet]
        public IActionResult CreateProduct()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Yeni Ürün Ekle";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            return View();
        }

        [Route("products/create")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid) return View(product);
            
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(productApiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla eklendi";
                    return RedirectToAction("Products");
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

        [Route("products/edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Ürün Düzenle";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
            try
            {
                var response = await _httpClient.GetAsync($"{productApiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(product);
                }
                TempData["Error"] = "Ürün bulunamadı";
                return RedirectToAction("Products");
            }
            catch (Exception)
            {
                TempData["Error"] = "Ürün yüklenirken bağlantı hatası oluştu";
                return RedirectToAction("Products");
            }
        }

        [Route("products/edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, Product product)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid) return View(product);
            
            try
            {
                product.Id = id;
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{productApiUrl}/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ürün başarıyla güncellendi";
                    return RedirectToAction("Products");
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

        [Route("products/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"{productApiUrl}/{id}");
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
            
            return RedirectToAction("Products");
        }

        // Orders Management
        [Route("orders")]
        public async Task<IActionResult> Orders()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Sipariş Yönetimi";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
            try
            {
                var response = await _httpClient.GetAsync(orderApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(orders ?? new List<Order>());
                }
                else
                {
                    TempData["Error"] = $"Siparişler yüklenirken hata oluştu: {response.StatusCode}";
                    return View(new List<Order>());
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Siparişler yüklenirken bağlantı hatası oluştu";
                return View(new List<Order>());
            }
        }

        [Route("orders/create")]
        [HttpGet]
        public IActionResult CreateOrder()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Yeni Sipariş";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            return View();
        }

        [Route("orders/create")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid) return View(order);
            
            try
            {
                var json = JsonSerializer.Serialize(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(orderApiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sipariş başarıyla eklendi";
                    return RedirectToAction("Orders");
                }
                else
                {
                    TempData["Error"] = "Sipariş eklenirken hata oluştu";
                    return View(order);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Sipariş eklenirken bağlantı hatası oluştu";
                return View(order);
            }
        }

        [Route("orders/delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"{orderApiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Sipariş başarıyla silindi";
                }
                else
                {
                    TempData["Error"] = "Sipariş silinirken hata oluştu";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Sipariş silinirken bağlantı hatası oluştu";
            }
            
            return RedirectToAction("Orders");
        }

        // Users Management
        [Route("users")]
        public async Task<IActionResult> Users()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Kullanıcı Yönetimi";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
            try
            {
                var response = await _httpClient.GetAsync(userApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(users ?? new List<User>());
                }
                else
                {
                    TempData["Error"] = $"Kullanıcılar yüklenirken hata oluştu: {response.StatusCode}";
                    return View(new List<User>());
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcılar yüklenirken bağlantı hatası oluştu";
                return View(new List<User>());
            }
        }

        [Route("users/create")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Yeni Kullanıcı";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            return View();
        }

        [Route("users/create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid) return View(user);
            
            try
            {
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(userApiUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kullanıcı başarıyla eklendi";
                    return RedirectToAction("Users");
                }
                else
                {
                    TempData["Error"] = "Kullanıcı eklenirken hata oluştu";
                    return View(user);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcı eklenirken bağlantı hatası oluştu";
                return View(user);
            }
        }

        [Route("users/delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"{userApiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kullanıcı başarıyla silindi";
                }
                else
                {
                    TempData["Error"] = "Kullanıcı silinirken hata oluştu";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Kullanıcı silinirken bağlantı hatası oluştu";
            }
            
            return RedirectToAction("Users");
        }
    }
} 