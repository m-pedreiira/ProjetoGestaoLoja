
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.DTO.Entidades;
using System.Linq.Expressions;

namespace ProjetoTresCamadas.Bussines.Services
{
    public class GestaoFuncionarios
    {
		private readonly IGenericRepository<Funcionario> _funcionarioRepository;

		public GestaoFuncionarios() : this(new GestaoFuncionarioRepository())
		{
		}

		public GestaoFuncionarios(IGenericRepository<Funcionario> funcionarioRepository)
		{
			_funcionarioRepository = funcionarioRepository;
		}

		public string SalvarFuncionario(Funcionario funcionario)
        {
            var Erro = "Funcionário salvo com Sucesso";
            try
            {
                if(funcionario.Id == 0)
                {
					_funcionarioRepository.Adicionar(funcionario);
                }
                else
                {
					_funcionarioRepository.Editar(funcionario);
                }
            }
            catch (Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public string RemoverFuncionario(Funcionario funcionario)
        {
            var Erro = "Funcionário removido com Sucesso";
            try
            {
                _funcionarioRepository.Excluir(funcionario);
                _funcionarioRepository.Salvar();
            }
            catch (Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public async Task<IList<Funcionario>> ObterFuncionarios()
        {
            return await _funcionarioRepository.ObterTodos();
        }
        public async Task<Funcionario> BuscarFuncionarioPor(Expression<Func<Funcionario, bool>> predicado)
        {
            return await _funcionarioRepository.BuscarPor(predicado);
        }
    }
}
