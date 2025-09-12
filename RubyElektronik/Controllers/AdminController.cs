using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubyElektronik.Models;
using RubyElektronik.Data;
using RubyElektronik.Services;
using Microsoft.AspNetCore.Http;

namespace RubyElektronik.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceRecordPdfService _pdfService;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
            _pdfService = new ServiceRecordPdfService();
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
                var productCount = await _context.Products.Where(p => p.IsActive).CountAsync();
                var orderCount = await _context.Orders.CountAsync();
                var userCount = await _context.Users.Where(u => u.IsActive).CountAsync();
                var serviceRecordCount = await _context.ServiceRecords.Where(s => s.IsActive).CountAsync();

                ViewBag.ProductCount = productCount;
                ViewBag.OrderCount = orderCount;
                ViewBag.UserCount = userCount;
                ViewBag.ServiceRecordCount = serviceRecordCount;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Dashboard verileri yüklenirken hata oluştu: {ex.Message}";
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
                var products = await _context.Products.Where(p => p.IsActive).ToListAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürünler yüklenirken hata oluştu: {ex.Message}";
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
                product.CreatedAt = DateTime.UtcNow;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla eklendi";
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün eklenirken hata oluştu: {ex.Message}";
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
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
                if (product == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Products");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün yüklenirken hata oluştu: {ex.Message}";
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
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Products");
                }
                
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.IsActive = product.IsActive;
                existingProduct.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla güncellendi";
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün güncellenirken hata oluştu: {ex.Message}";
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
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Products");
                }
                
                product.IsActive = false;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün silinirken hata oluştu: {ex.Message}";
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
                var orders = await _context.Orders.ToListAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Siparişler yüklenirken hata oluştu: {ex.Message}";
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
                order.CreatedAt = DateTime.UtcNow;
                order.TotalPrice = order.UnitPrice * order.Quantity;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla eklendi";
                return RedirectToAction("Orders");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş eklenirken hata oluştu: {ex.Message}";
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
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı";
                    return RedirectToAction("Orders");
                }
                
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş silinirken hata oluştu: {ex.Message}";
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
                var users = await _context.Users.Where(u => u.IsActive).ToListAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kullanıcılar yüklenirken hata oluştu: {ex.Message}";
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

            // Custom validation for company name
            if (user.UserType == UserType.Corporate && string.IsNullOrWhiteSpace(user.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "Kurumsal kullanıcılar için firma adı zorunludur");
            }

            if (!ModelState.IsValid) return View(user);
            
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kullanıcı başarıyla eklendi";
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kullanıcı eklenirken hata oluştu: {ex.Message}";
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
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bulunamadı";
                    return RedirectToAction("Users");
                }
                
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kullanıcı başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kullanıcı silinirken hata oluştu: {ex.Message}";
            }
            
            return RedirectToAction("Users");
        }

        // Service Records Management
        [Route("servicerecords")]
        public async Task<IActionResult> ServiceRecords()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            ViewData["Title"] = "Ruby Elektronik - Servis Kayıtları Yönetimi";
            ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
            
            try
            {
                var serviceRecords = await _context.ServiceRecords.ToListAsync();
                return View(serviceRecords);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Servis kayıtları yüklenirken hata oluştu: {ex.Message}";
                return View(new List<ServiceRecord>());
            }
        }

        [Route("servicerecords/complete/{id}")]
        public async Task<IActionResult> CompleteServiceRecord(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var serviceRecord = await _context.ServiceRecords.FindAsync(id);
                if (serviceRecord == null)
                {
                    TempData["Error"] = "Servis kaydı bulunamadı";
                    return RedirectToAction("ServiceRecords");
                }
                
                serviceRecord.IsActive = false;
                serviceRecord.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servis kaydı başarıyla tamamlandı";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Servis kaydı tamamlanırken hata oluştu: {ex.Message}";
            }
            
            return RedirectToAction("ServiceRecords");
        }

        [Route("servicerecords/delete/{id}")]
        public async Task<IActionResult> DeleteServiceRecord(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var serviceRecord = await _context.ServiceRecords.FindAsync(id);
                if (serviceRecord == null)
                {
                    TempData["Error"] = "Servis kaydı bulunamadı";
                    return RedirectToAction("ServiceRecords");
                }
                
                _context.ServiceRecords.Remove(serviceRecord);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servis kaydı tamamen silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Servis kaydı silinirken hata oluştu: {ex.Message}";
            }
            
            return RedirectToAction("ServiceRecords");
        }

        // PDF İndirme Endpoint'leri
        [Route("servicerecords/pdf/{id}")]
        public async Task<IActionResult> DownloadServiceRecordPdf(int id)
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var serviceRecord = await _context.ServiceRecords.FindAsync(id);
                if (serviceRecord != null)
                {
                    var pdfBytes = _pdfService.GenerateServiceRecordPdf(serviceRecord);
                    var fileName = $"ServisKaydi_{serviceRecord.Ad}_{serviceRecord.Soyad}_{serviceRecord.Id}_{DateTime.Now:yyyyMMdd}.html";
                    
                    return File(pdfBytes, "text/html", fileName);
                }
                
                TempData["Error"] = "Servis kaydı bulunamadı";
                return RedirectToAction("ServiceRecords");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"PDF oluşturulurken hata oluştu: {ex.Message}";
                return RedirectToAction("ServiceRecords");
            }
        }

        [Route("servicerecords/pdf/all")]
        public async Task<IActionResult> DownloadAllServiceRecordsPdf()
        {
            // Giriş kontrolü
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var serviceRecords = await _context.ServiceRecords.ToListAsync();
                
                if (serviceRecords != null && serviceRecords.Any())
                {
                    var pdfBytes = _pdfService.GenerateServiceRecordsPdf(serviceRecords);
                    var fileName = $"ServisKayitlari_Raporu_{DateTime.Now:yyyyMMdd_HHmm}.html";
                    
                    return File(pdfBytes, "text/html", fileName);
                }
                
                TempData["Error"] = "Servis kayıtları bulunamadı";
                return RedirectToAction("ServiceRecords");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"PDF oluşturulurken hata oluştu: {ex.Message}";
                return RedirectToAction("ServiceRecords");
            }
        }
    }
}