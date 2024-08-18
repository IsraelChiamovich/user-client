using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using user_client.ViewModel;

namespace user_client.Controllers
{
    public class UserClientController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string baseUrl = "https://localhost:7144/api/User";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<UserVM>? users = JsonSerializer.Deserialize<List<UserVM>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(users);
            }
            return RedirectToAction("Index", "Home");
        }


        
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{baseUrl}/{id}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                UserVM? user = JsonSerializer.Deserialize<UserVM>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM user)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"{baseUrl}/Create", httpContent);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(new {Email, Password}), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"{baseUrl}/auth", httpContent);
            if (result.IsSuccessStatusCode)
            {  
                return RedirectToAction("Index");
            }
            return RedirectToAction("AuthError");
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{baseUrl}/{id}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                UserVM? user = JsonSerializer.Deserialize<UserVM>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserVM userVM)
        {
            int id = userVM.Id;
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(userVM), Encoding.UTF8, "application/json");
            var result = await httpClient.PutAsync($"{baseUrl}/update/{id}", httpContent);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.DeleteAsync($"{baseUrl}/delete/{id}");
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AuthError()
        {
            return View();
        }
    }
}
