using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubyElektronik.Models;
using RubyElektronik.Data;

namespace RubyElektronik.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Kullanıcı eklenirken hata oluştu: {ex.Message}";
                return View(user);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bulunamadı";
                    return RedirectToAction("Index");
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
            
            return RedirectToAction("Index");
        }
    }
} 