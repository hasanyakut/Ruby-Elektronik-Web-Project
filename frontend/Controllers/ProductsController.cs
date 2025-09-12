using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using frontend.Models;
using frontend.Data;

namespace frontend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
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
                TempData["Error"] = $"Ürünler yüklenirken hata oluştu: {ex.Message}";
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
                product.CreatedAt = DateTime.UtcNow;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla eklendi";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün eklenirken hata oluştu: {ex.Message}";
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
                if (product == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (!ModelState.IsValid) return View(product);
            
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Index");
                }
                
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.IsActive = product.IsActive;
                existingProduct.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla güncellendi";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ürün güncellenirken hata oluştu: {ex.Message}";
                return View(product);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["Error"] = "Ürün bulunamadı";
                    return RedirectToAction("Index");
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
            
            return RedirectToAction("Index");
        }
    }
} 