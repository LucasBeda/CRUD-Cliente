using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class EditarLogradouroController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public EditarLogradouroController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI"));
        }

        public IActionResult Index(int id)
        {
            Logradouro logradouro = new Logradouro();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"Logradouro/Buscar/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string logradourosAPI = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(logradourosAPI))
                {
                    List<Logradouro> logradouros = JsonConvert.DeserializeObject<List<Logradouro>>(logradourosAPI);
                    logradouro = logradouros.FirstOrDefault();
                }
            }

            return View(logradouro);
        }

        public IActionResult Atualizar(Logradouro logradouroAtualizado)
        {
            string requestJSON = JsonConvert.SerializeObject(logradouroAtualizado);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(requestJSON);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "Logradouro/Atualizar", content).Result;
            return LocalRedirect("/ConsultarCliente");
        }
    }
}
