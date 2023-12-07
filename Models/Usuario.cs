using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CRUD_Cliente.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public Usuario() { }
        public Usuario(string nome, string senha)
        {
            Nome = nome;
            Senha = senha;
        }

        public bool ValidarUsuario(Usuario usuario)
        {
            return usuario.Nome.ToLower().Equals("lucas") && usuario.Senha.Equals("123");
        }
    }
}
