using Microsoft.AspNetCore.Mvc;
using frontend.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace frontend.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "http://localhost:5145/users";

        public UsersController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(apiUrl);
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(users);
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
            
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(apiUrl, content);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"{apiUrl}/{id}");
            return RedirectToAction("Index");
        }
    }
} 