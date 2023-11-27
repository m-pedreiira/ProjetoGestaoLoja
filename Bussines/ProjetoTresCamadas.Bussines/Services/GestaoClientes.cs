

using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.DTO.Entidades;
using System.Linq.Expressions;

namespace ProjetoTresCamadas.Bussines.Services
{
    public class GestaoClientes :IDisposable 
    {
		private readonly IGenericRepository<Cliente> _clienteRepository;

		public GestaoClientes() : this(new GestaoClienteRepository())
		{
		}

		public GestaoClientes(IGenericRepository<Cliente> clienteRepository)
		{
			_clienteRepository = clienteRepository;
		}
		public string SalvarCliente(Cliente cliente)
        {
            var erro = "Cliente salvo no banco de dados com Sucesso";
            try
            {
                if(cliente.Id == 0)
                {
					_clienteRepository.Adicionar(cliente);
                }
                else
                {
					_clienteRepository.Editar(cliente);
                    erro = "A edicão do cliente no banco de dados foi feita com sucesso!";
                }
                
                _clienteRepository.Salvar();
                _clienteRepository.Dispose();
            }
            catch (Exception e)
            {
                erro = e.Message;
            }
            return erro;
        }
        public string RemoverCliente(Cliente cliente)
        {
            var erro = "Cliente removido do banco de dados com sucesso!";
            try
            {
                _clienteRepository.Excluir(cliente);
            }
            catch (Exception e)
            {
                erro = e.Message;
            }
            return erro;
        }
        public async Task<IList<Cliente>> ObterClientes()
        {
                var clientes = await _clienteRepository.ObterTodos();
                return clientes;
        }
        public async Task<Cliente> BuscarCliente(Expression<Func<Cliente, bool>> predicado)
        {
            return await _clienteRepository.BuscarPor(predicado);
        }

        public void Dispose()
        {
            _clienteRepository.Dispose();
        }
    }
}
