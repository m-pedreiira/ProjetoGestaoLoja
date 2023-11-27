

using System.Linq.Expressions;
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.DTO.Entidades;

namespace ProjetoTresCamadas.Bussines.Services
{
    public class GestaoCategorias
    {
        private readonly IGenericRepository<Categoria> _categoriaRepository;

		public GestaoCategorias() : this(new GestaoCategoriasRepository())
		{
		}

		public GestaoCategorias(IGenericRepository<Categoria> categoriaRepository)
		{
			_categoriaRepository = categoriaRepository;
		}

		public string SalvarCategoria(Categoria categoria)
        {
            var Erro = "Categoria salva com sucesso!";
            try
            {
                if(categoria.Id == 0)
                {
                    _categoriaRepository.Adicionar(categoria);
                    
                }
                else
                {
                    _categoriaRepository.Editar(categoria);
                    Erro = "Categoria editada com sucesso!";
                }
            }catch(Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public string ExcluirCategoria(Categoria categoria)
        {
            var Erro = "Categoria excluida com sucesso!";
            try
            {
                _categoriaRepository.Excluir(categoria);
            }catch(Exception e)
            {
                Erro = e.Message;
            }
            return Erro;
        }
        public async Task<IList<Categoria>> ObterCategorias()
        {
            var categorias = _categoriaRepository.ObterTodos().Result.ToList();
            return categorias;
        }
        public async Task<Categoria> BuscarCategoriaPor(Expression<Func<Categoria, bool>> predicado)
        {
            return await _categoriaRepository.BuscarPor(predicado);
        }
    }
}
