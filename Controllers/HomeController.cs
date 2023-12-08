using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;

namespace CRUD_Cliente.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public HomeController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI") + "Token");
        }

        public IActionResult Index()
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
            //return View("~/Views/CadastrarCliente/Index.cshtml");
            return LocalRedirect("/CadastrarCliente");
        }

        public IActionResult Logout()
        {
            _tokenGlobal.Token = string.Empty;
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}