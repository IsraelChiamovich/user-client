using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using user_client.ViewModel;
using user_client.Models;
using System.Net.Http.Headers;
using user_client.Utils;
using user_client.Dto;

namespace user_client.Controllers
{
    public class UserClientController(IHttpClientFactory clientFactory, Authentication auth) : Controller
    {
        private readonly string baseUrl = "https://localhost:7144/api/User";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync(baseUrl);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<UserDto>? users = JsonSerializer.Deserialize<List<UserDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(users);
            }
            return RedirectToAction("Index", "Home");
        }


        
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                UserDto? user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(user);
            }
            return View("AuthError");
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
            var httpContent = new StringContent(JsonSerializer.Serialize(new
            {
                user.Email,
                user.Password,
                user.Name,
                Image = ImageUtils.ConvertFromIFormFile(user.Image)
            }),
                Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/Create");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
                request.Content = httpContent;
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return View("AuthError");
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
            var httpContent = new StringContent(JsonSerializer.Serialize(new {Email, Password,}), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"{baseUrl}/auth", httpContent);
            if (result.IsSuccessStatusCode)
            {
                auth.Token = await result.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return View("LoginError");
            
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
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/update/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            request.Content = httpContent;
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return View("AuthError");
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
