using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using frontend.Models;
using frontend.Data;

namespace frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid) return View(order);
            
            try
            {
                order.CreatedAt = DateTime.UtcNow;
                order.TotalPrice = order.UnitPrice * order.Quantity;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla eklendi";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş eklenirken hata oluştu: {ex.Message}";
                return View(order);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı";
                    return RedirectToAction("Index");
                }
                
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Sipariş silinirken hata oluştu: {ex.Message}";
            }
            
            return RedirectToAction("Index");
        }
    }
} 