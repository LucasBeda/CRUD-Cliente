using CRUD_Cliente.Infra.Repositorio;
using System.Text.Json.Serialization;

namespace CRUD_Cliente.Models
{
    public class Logradouro
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int LogradouroId { get; set; }
        public string Endereco { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Cliente Cliente { get; set; }
        public Logradouro() { }

        public List<string> ValidarLogradouro(Logradouro logradouro, IRepositorio repo)
        {
            var erros = new List<string>();

            if (logradouro.Cliente == null || logradouro.Cliente.ClienteId == 0)
                erros.Add("Campo obrigatório não preenchido: Id do Cliente");

            if (string.IsNullOrEmpty(logradouro.Endereco))
                erros.Add("Campo obrigatório não preenchido: Endereço");

            if (logradouro.Cliente != null)
            {
                List<Logradouro> logradourosCliente = repo.ObterLogradouroPorCliente(logradouro.Cliente.ClienteId);

                foreach (var item in logradourosCliente)
                {
                    if (item.Endereco == logradouro.Endereco)
                        erros.Add("Cliente já possui esse endereço cadastrado");
                }
            }

            return erros;
        }
    }
}