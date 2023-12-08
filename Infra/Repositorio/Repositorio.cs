using CRUD_Cliente.Controllers;
using CRUD_Cliente.Models;
using Dapper;
using System.Data;

namespace CRUD_Cliente.Infra.Repositorio
{
    public class Repositorio : IRepositorio
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<Repositorio> _log;

        public Repositorio(IDbConnection dbConnection, ILogger<Repositorio> logger)
        {
            _dbConnection = dbConnection;
            _log = logger;
        }

        public async Task<IEnumerable<Cliente>> ObterAsyncCliente(long id = 0)
        {
            string sql = "SELECT ClienteId, Nome, Email, Logotipo FROM Cliente cli ";

            if (id > 0)
                sql += $"WHERE ClienteId = @id";

            IEnumerable<Cliente> clientes = await _dbConnection.QueryAsync<Cliente>(sql,
                                                                new
                                                                {
                                                                    id = id
                                                                });

            return clientes;
        }

        public async Task<List<Cliente>> ObterAsyncClienteCompleto()
        {
            string sql = @"SELECT LogradouroId, Endereco, log.ClienteId, Nome, Email, Logotipo 
                          FROM Logradouro log 
                          INNER JOIN Cliente cli ON (log.ClienteId = cli.ClienteId) ";

            Dictionary<int, Cliente> dicCliente = new Dictionary<int, Cliente>();

            IEnumerable<Cliente> clientesAsync = await _dbConnection.QueryAsync<Logradouro, Cliente, Cliente>(sql, (_logradouro, _cliente) =>
            {
                Cliente? clienteQuery;

                if (!dicCliente.TryGetValue(_cliente.ClienteId, out clienteQuery))
                {
                    clienteQuery = _cliente;
                    clienteQuery.Enderecos = new List<Logradouro>();
                    dicCliente.TryAdd(_cliente.ClienteId, clienteQuery);
                }

                if (_logradouro != null)
                {
                    clienteQuery.Enderecos.Add(_logradouro);
                }

                if (_cliente.Enderecos != null && _cliente.Enderecos.Count() > 0)
                    return _cliente;
                else
                    return null;
            }, splitOn: "ClienteId");

            var clientes = clientesAsync.Distinct().ToList();
            clientes.Remove(null);

            return clientes;
        }

        public Cliente ObterCliente(long id)
        {
            string sql = "SELECT ClienteId, Nome, Email, Logotipo FROM Cliente WHERE ClienteId = @id";

            Cliente cliente = _dbConnection.Query<Cliente>(sql,
                                                new
                                                {
                                                    id = id
                                                }).First();

            return cliente;
        }

        public List<Logradouro> ObterLogradouro(long id)
        {
            string sql = "SELECT LogradouroId, Endereco, ClienteId FROM Logradouro WHERE LogradouroId = @id";

            List<Logradouro> logradouros = _dbConnection.Query<Logradouro>(sql,
                                                             new
                                                             {
                                                                 id = id
                                                             }).ToList();

            return logradouros;
        }

        public List<Logradouro> ObterLogradouroPorCliente(long idCliente)
        {
            string sql = "SELECT LogradouroId, Endereco, ClienteId FROM Logradouro WHERE ClienteId = @id";

            List<Logradouro> logradouros = _dbConnection.Query<Logradouro>(sql,
                                                             new
                                                             {
                                                                 id = idCliente
                                                             }).ToList();

            return logradouros;
        }

        public int CadastrarCliente(Cliente cliente)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@nome", cliente.Nome);
            param.Add("@email", cliente.Email);
            param.Add("@logotipo", cliente.Logotipo);
            param.Add("@clienteId", DbType.Int32, direction: ParameterDirection.Output);
            _dbConnection.Execute("CadastrarCliente", param, commandType: CommandType.StoredProcedure);
            return param.Get<int>("@clienteId");
        }

        public int CadastrarLogradouro(Logradouro endereco)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@endereco", endereco.Endereco);
            param.Add("@clienteId", endereco.Cliente.ClienteId);
            param.Add("@logradouroId", DbType.Int32, direction: ParameterDirection.Output);
            _dbConnection.Execute("CadastrarLogradouro", param, commandType: CommandType.StoredProcedure);
            return param.Get<int>("@logradouroId");
        }

        public void AtualizarCliente(Cliente cliente)
        {
            //_ = _dbConnection.Execute("UPDATE Cliente SET nome = @nome, email = @email, logotipo = @logotipo WHERE ClienteId = @id",
            //                 new
            //                 {
            //                     nome = cliente.Nome,
            //                     email = cliente.Email,
            //                     logotipo = cliente.Logotipo,
            //                     id = cliente.ClienteId
            //                 });
            DynamicParameters param = new DynamicParameters();
            param.Add("@clienteId", cliente.ClienteId);
            param.Add("@nome", cliente.Nome);
            param.Add("@email", cliente.Email);
            param.Add("@logotipo", cliente.Logotipo);
            _dbConnection.Execute("AtualizarCliente", param, commandType: CommandType.StoredProcedure);
        }

        public void AtualizarLogradouro(Logradouro endereco)
        {
            //_ = _dbConnection.Execute("UPDATE Logradouro SET endereco = @endereco WHERE LogradouroId = @id",
            //                 new
            //                 {
            //                     endereco = endereco.Endereco,
            //                     id = endereco.LogradouroId
            //                 });
            DynamicParameters param = new DynamicParameters();
            param.Add("@logradouroId", endereco.LogradouroId);
            param.Add("@endereco", endereco.Endereco);
            _dbConnection.Execute("AtualizarLogradouro", param, commandType: CommandType.StoredProcedure);
        }

        public void RemoverCliente(long id)
        {
            try
            {
                //_ = _dbConnection.Execute("DELETE FROM Cliente WHERE ClienteId = @id", 
                //                 new 
                //                 {
                //                     id = id 
                //                 });
                DynamicParameters param = new DynamicParameters();
                param.Add("@clienteId", id);
                _dbConnection.Execute("RemoverCliente", param, commandType: CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                _log.LogError($"RemoverCliente - Erro: {ex.Message}");
                throw;
            }
        }

        public void RemoverLogradouro(long id)
        {
            try
            {
                //_ = _dbConnection.Execute("DELETE FROM Logradouro WHERE ClienteId = @id",
                //                 new
                //                 {
                //                     id = id
                //                 });
                DynamicParameters param = new DynamicParameters();
                param.Add("@logradouroId", id);
                _dbConnection.Execute("RemoverLogradouro", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _log.LogError($"RemoverLogradouro - Erro: {ex.Message}");
                throw;
            }
        }
    }
}
