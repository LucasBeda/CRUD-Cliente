using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace CRUD_Cliente.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI") + "Token");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Usuario usuarioLogin)
        {
            string requestJSON = JsonSerializer.Serialize(usuarioLogin);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(requestJSON);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress, content).Result;

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            _tokenGlobal.Token = response.Content.ReadAsStringAsync().Result;
            return RedirectToPage("../Cliente/Index/");
        }

        public IActionResult Logout()
        {
            _tokenGlobal.Token = string.Empty;
            return RedirectToAction("Login/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}