using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class ConsultarLogradouroController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public ConsultarLogradouroController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI"));
        }

        public IActionResult Index(int idCliente)
        {
            List<Logradouro> logradouros = new List<Logradouro>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"Logradouro/BuscarPorCliente/{idCliente}").Result;

            if (response.IsSuccessStatusCode)
            {
                string logradourosAPI = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(logradourosAPI))
                    logradouros = JsonConvert.DeserializeObject<List<Logradouro>>(logradourosAPI);
            }

            return View(logradouros);
        }

        public IActionResult Edit(int id)
        {
            return RedirectToAction("Index", "EditarLogradouro", new { id = id });
        }

        public IActionResult Delete(int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"Logradouro/Remover/{id}").Result;
            return LocalRedirect("/ConsultarCliente");
        }
    }
}
