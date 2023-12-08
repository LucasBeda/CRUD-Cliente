using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class EditarClienteController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public EditarClienteController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI"));
        }

        public IActionResult Index(int id)
        {
            Cliente cliente = new Cliente();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"Cliente/Buscar/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string clientesAPI = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(clientesAPI))
                {
                    List<Cliente> clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesAPI);
                    cliente = clientes.FirstOrDefault();
                }
            }

            return View(cliente);
        }

        public IActionResult Atualizar(Cliente clienteAtualizado)
        {
            string requestJSON = JsonConvert.SerializeObject(clienteAtualizado);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(requestJSON);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "Cliente/Atualizar", content).Result;
            return LocalRedirect("/ConsultarCliente");
        }
    }
}
