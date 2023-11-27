using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetoTresCamadas.Bussines.Services;
using ProjetoTresCamadas.DTO.Entidades;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MeusTestes.Testes.Services
{
	[TestClass]
	public class GestaoCategoriasTestes
	{
		private GestaoCategorias _gestaoCategorias;
		private Mock<IGenericRepository<Categoria>> _mockRepositorio;

		[TestInitialize]
		public void PrepararTeste()
		{
			_mockRepositorio = new Mock<IGenericRepository<Categoria>>();
			_gestaoCategorias = new GestaoCategorias(_mockRepositorio.Object);
		}

		[TestMethod]
		public void SalvarCategoria_NovaCategoria_test()
		{
			var categoria = new Categoria
			{
				Id = 0,
				TipoCategoria = "Eletrônicos"
			};

			var resultado = _gestaoCategorias.SalvarCategoria(categoria);

			Assert.AreEqual("Categoria salva com sucesso!", resultado);
			_mockRepositorio.Verify(repo => repo.Adicionar(categoria), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Never);
		}

		[TestMethod]
		public void SalvarCategoria_CategoriaExistente_test()
		{
			var categoria = new Categoria
			{
				Id = 1,
				TipoCategoria = "Eletrodomésticos"
			};

			var resultado = _gestaoCategorias.SalvarCategoria(categoria);

			Assert.AreEqual("Categoria editada com sucesso!", resultado);
			_mockRepositorio.Verify(repo => repo.Editar(categoria), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Never);
		}

		[TestMethod]
		public void ExcluirCategoria_test()
		{
			var categoria = new Categoria
			{
				Id = 1,
				TipoCategoria = "Eletrodomésticos"
			};

			var resultado = _gestaoCategorias.ExcluirCategoria(categoria);

			Assert.AreEqual("Categoria excluida com sucesso!", resultado);
			_mockRepositorio.Verify(repo => repo.Excluir(categoria), Times.Once);
		}

		[TestMethod]
		public async Task ObterCategorias_test()
		{
			var categoriasSimuladas = new List<Categoria>
			{
				new Categoria { Id = 1, TipoCategoria = "Eletrônicos" },
				new Categoria { Id = 2, TipoCategoria = "Eletrodomésticos" }
			};
			_mockRepositorio.Setup(repo => repo.ObterTodos()).ReturnsAsync(categoriasSimuladas);

			var resultado = await _gestaoCategorias.ObterCategorias();

			Assert.IsNotNull(resultado);
			Assert.AreEqual(2, resultado.Count);
		}

		[TestMethod]
		public async Task BuscarCategoriaPor_test()
		{
			Expression<Func<Categoria, bool>> predicado = c => c.Id == 1;
			var categoriaSimulada = new Categoria { Id = 1, TipoCategoria = "Eletrônicos" };
			_mockRepositorio.Setup(repo => repo.BuscarPor(predicado)).ReturnsAsync(categoriaSimulada);

			var resultado = await _gestaoCategorias.BuscarCategoriaPor(predicado);

			Assert.IsNotNull(resultado);
			Assert.AreEqual(1, resultado.Id);
			Assert.AreEqual("Eletrônicos", resultado.TipoCategoria);
		}
	}
}