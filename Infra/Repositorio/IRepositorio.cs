using CRUD_Cliente.Models;

namespace CRUD_Cliente.Infra.Repositorio
{
    public interface IRepositorio
    {
        Task<IEnumerable<Cliente>> ObterAsyncCliente(long id = 0);
        Task<List<Cliente>> ObterAsyncClienteCompleto();
        Cliente ObterCliente(long id);
        List<Logradouro> ObterLogradouroPorCliente(long idCliente);
        int CadastrarCliente(Cliente cliente);
        int CadastrarLogradouro(Logradouro endereco);
        void AtualizarCliente(Cliente cliente);
        void AtualizarLogradouro(Logradouro endereco);
        void RemoverCliente(long id);
        void RemoverLogradouro(long id);
    }
}
