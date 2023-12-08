using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class ConsultarClienteController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public ConsultarClienteController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI"));
        }

        public IActionResult Index()
        {
            List<Cliente> clientes = new List<Cliente>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Cliente/Buscar").Result;

            if (response.IsSuccessStatusCode)
            {
                string clientesAPI = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(clientesAPI))
                    clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesAPI);
            }

            return View(clientes);
        }

        public IActionResult CadastrarLogradouro(int id)
        {
            return RedirectToAction("Index", "CadastrarLogradouro", new { idCliente = id });
        }

        public IActionResult ConsultarLogradouro(int id)
        {
            return RedirectToAction("Index", "ConsultarLogradouro", new { idCliente = id });
        }

        public IActionResult Edit(int id)
        {
            return RedirectToAction("Index", "EditarCliente", new { id = id });
        }

        public IActionResult Delete(int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"Cliente/Remover/{id}").Result;
            return RedirectToAction("Index");
        }
    }
}
