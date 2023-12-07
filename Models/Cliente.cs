using CRUD_Cliente.Infra.Repositorio;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CRUD_Cliente.Models
{
    public class Cliente
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Logotipo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Logradouro>? Enderecos { get; set; }
        public Cliente() { }

        public List<string> ValidarCliente(Cliente cliente, IRepositorio repo)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(cliente.Nome))
                erros.Add("Campo obrigatório não preenchido: Nome");

            if (string.IsNullOrEmpty(cliente.Email))
                erros.Add("Campo obrigatório não preenchido: E-mail");

            var clientesDB = repo.ObterAsyncCliente();

            foreach(var item in clientesDB.Result)
            {
                if (item.Email == cliente.Email)
                {
                    if (cliente.ClienteId == 0 || item.ClienteId != cliente.ClienteId)
                    {
                        erros.Add("Já existe um cliente cadastrado com esse email");
                        break;
                    }
                }
            }            

            return erros;
        }
    }
}