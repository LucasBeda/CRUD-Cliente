using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CRUD_Cliente.Controllers
{
    public class CadastrarClienteController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationToken _tokenGlobal;
        private readonly HttpClient _client;

        public CadastrarClienteController(IConfiguration configuration, ApplicationToken tokenGlobal)
        {
            _configuration = configuration;
            _tokenGlobal = tokenGlobal;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.GetValue<string>("EndPointAPI") + "Cliente/Cadastrar");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastro(Cliente clienteCadastro)
        {
            if (string.IsNullOrEmpty(_tokenGlobal.Token))
                return LocalRedirect("/Home");
            //return View("~/Views/Home/Index.cshtml");

            string requestJSON = JsonConvert.SerializeObject(clienteCadastro);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(requestJSON);
            var content = new ByteArrayContent(messageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenGlobal.Token);
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress, content).Result;

            return LocalRedirect("/CadastrarCliente");
        }       
    }
}
