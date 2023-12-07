using CRUD_Cliente.Infra.Repositorio;
using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Cliente.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _log;
        private readonly IRepositorio _repositorio;
        private readonly ApplicationToken _tokenGlobal;
        public ClienteController(ILogger<ClienteController> logger, IRepositorio repositorio, ApplicationToken tokenGlobal)
        {
            _log = logger;
            _repositorio = repositorio;
            _tokenGlobal = tokenGlobal;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Cadastrar")]
        public IActionResult CadastrarCliente([FromBody] Cliente cliente)
        {
            try
            {
                List<string> erros = new Cliente().ValidarCliente(cliente, _repositorio);
                if (erros.Count() > 0)
                    return StatusCode(StatusCodes.Status400BadRequest, erros);

                int id = _repositorio.CadastrarCliente(cliente);
                return Ok($"Cliente cadastrado com sucesso, ID: {id}");
            }
            catch(Exception ex)
            {
                _log.LogError($"CadastrarCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("Atualizar")]
        public IActionResult AtualizarCliente([FromBody] Cliente cliente)
        {
            try
            {
                List<string> erros = new Cliente().ValidarCliente(cliente, _repositorio);
                if (erros.Count() > 0)
                    return StatusCode(StatusCodes.Status400BadRequest, erros);

                _repositorio.AtualizarCliente(cliente);
                return Ok("Cliente atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _log.LogError($"AtualizarCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("Remover/{id}")]
        public IActionResult RemoverCliente(int id)
        {
            try
            {
                _repositorio.RemoverLogradouro(id);
                _repositorio.RemoverCliente(id);
                return Ok("Cliente removido com sucesso.");
            }
            catch (Exception ex)
            {
                _log.LogError($"RemoverCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("Buscar/{id}")]
        public IActionResult BuscarCliente(int id)
        {
            try
            {
                Task<IEnumerable<Cliente>> clientes = _repositorio.ObterAsyncCliente(id);
                return Ok(clientes.Result);
            }
            catch (Exception ex)
            {
                _log.LogError($"BuscarCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("Buscar")]
        public IActionResult BuscarTodosCliente()
        {
            try
            {
                Task<IEnumerable<Cliente>> clientes = _repositorio.ObterAsyncCliente();
                return Ok(clientes.Result);
            }
            catch (Exception ex)
            {
                _log.LogError($"BuscarCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("BuscarInfoClienteCompleto")]
        public IActionResult BuscarInfoClienteCompleto()
        {
            try
            {
                Task<List<Cliente>> clientes = _repositorio.ObterAsyncClienteCompleto();
                return Ok(clientes.Result);
            }
            catch (Exception ex)
            {
                _log.LogError($"BuscarCliente - Erro: {ex.Message}");
                throw;
            }
        }
    }
}