using System.Linq.Expressions;
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.DTO.Entidades;

namespace ProjetoTresCamadas.Bussines.Services
{
    public class GestaoProdutos
    {
		private readonly IGenericRepository<Produto> _produtosRepository;

		public GestaoProdutos() : this(new GestaoProdutosRepository())
		{
		}

		public GestaoProdutos(IGenericRepository<Produto> produtosRepository)
		{
			_produtosRepository = produtosRepository;
		}
		public string SalvarProduto(Produto produto)
        {
            var Erro = "Produto salvo com sucesso!";
            try
            {
                if(produto.Id == 0)
                {
					_produtosRepository.Adicionar(produto);
                }
                else
                {
					_produtosRepository.Editar(produto);
                    Erro = "Produto Editado com sucesso!";
                }
                
            }
            catch(Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public string ExcluirProduto(Produto produto)
        {
            var Erro = "Produto excluido com sucesso";
            try
            {
                _produtosRepository.Excluir(produto);
            }catch(Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public async Task<IList<Produto>> ObterProdutos()
        {
            return await _produtosRepository.ObterTodos();
        }
        public async Task<Produto> BuscarProdutoPor(Expression<Func<Produto, bool>> predicado)
        {
            return await _produtosRepository.BuscarPor(predicado);
        }
        
    }
}
