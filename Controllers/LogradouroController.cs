using CRUD_Cliente.Infra.Repositorio;
using CRUD_Cliente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Cliente.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogradouroController : Controller
    {
        private readonly ILogger<LogradouroController> _log;
        private readonly IRepositorio _repositorio;
        public LogradouroController(ILogger<LogradouroController> logger, IRepositorio repositorio)
        {
            _log = logger;
            _repositorio = repositorio;
        }

        [HttpGet("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            try
            {
                List<Logradouro> logradouros = _repositorio.ObterLogradouro(id);
                return Ok(logradouros);
            }
            catch (Exception ex)
            {
                _log.LogError($"Buscar - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("BuscarPorCliente/{idCliente}")]
        public IActionResult BuscarPorCliente(int idCliente)
        {
            try
            {
                List<Logradouro> logradouros = _repositorio.ObterLogradouroPorCliente(idCliente);
                return Ok(logradouros);
            }
            catch (Exception ex)
            {
                _log.LogError($"BuscarPorCliente - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost("Cadastrar")]
        public IActionResult CadastrarLogradouro([FromBody] Logradouro endereco)
        {
            try
            {
                List<string> erros = new Logradouro().ValidarLogradouro(endereco, _repositorio);
                if (erros.Count() > 0)
                    return StatusCode(StatusCodes.Status400BadRequest, erros);

                int id = _repositorio.CadastrarLogradouro(endereco);
                return Ok($"Logradouro cadastrado com sucesso, ID: {id}");
            }
            catch (Exception ex)
            {
                _log.LogError($"CadastrarLogradouro - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("Atualizar")]
        public IActionResult AtualizarLogradouro([FromBody] Logradouro endereco)
        {
            try
            {
                List<string> erros = new Logradouro().ValidarLogradouro(endereco, _repositorio);
                if (erros.Count() > 0)
                    return StatusCode(StatusCodes.Status400BadRequest, erros);

                _repositorio.AtualizarLogradouro(endereco);
                return Ok("Logradouro atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _log.LogError($"AtualizarLogradouro - Erro: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("Remover/{id}")]
        public IActionResult RemoverLogradouro(int id)
        {
            try
            {
                _repositorio.RemoverLogradouro(id);
                return Ok("Logradouro removido com sucesso.");
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError($"RemoverLogradouro - Erro: {ex.Message}");
                throw;
            }
        }
    }
}
