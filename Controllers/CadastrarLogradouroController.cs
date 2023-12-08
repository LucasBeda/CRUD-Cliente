using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class CadastrarLogradouroController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public CadastrarLogradouroController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI") + "Logradouro/Cadastrar");
        }

        public IActionResult Index(int idCliente)
        {
            return View(new Logradouro()
            {
                Cliente = new Cliente()
                {
                    ClienteId = idCliente
                }
            });
        }

        public IActionResult Cadastro(Logradouro enderecoCadastro)
        {
            if (string.IsNullOrEmpty(_tokenGlobal.Token))
                return LocalRedirect("/Home");
            //return View("~/Views/Home/Index.cshtml");

            string requestJSON = JsonConvert.SerializeObject(enderecoCadastro);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(requestJSON);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress, content).Result;

            return LocalRedirect("/ConsultarCliente");
        }
    }
}
