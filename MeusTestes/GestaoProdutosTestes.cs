using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.Bussines.Services;
using ProjetoTresCamadas.DTO.Entidades;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MeusTestes.Testes.Services
{
	[TestClass]
	public class GestaoProdutosTestes
	{
		private GestaoProdutos _gestaoProdutos;
		private Mock<IGenericRepository<Produto>> _mockRepositorio;

		[TestInitialize]
		public void PrepararTeste()
		{
			_mockRepositorio = new Mock<IGenericRepository<Produto>>();
			_gestaoProdutos = new GestaoProdutos(_mockRepositorio.Object);
		}

		[TestMethod]
		public void SalvarProduto_NovoProduto_test()
		{
			var produto = new Produto
			{
				Id = 0,
				Nome = "Televisao",
				Preco = 300,
				Descricao = "TV LG",
				Estoque = 50,
				CategoriaId = 1
			};

			_gestaoProdutos.SalvarProduto(produto);

			_mockRepositorio.Verify(repo => repo.Adicionar(produto), Times.Once);
		}

		[TestMethod]
		public void SalvarProduto_ProdutoExistente_test()
		{
			var produto = new Produto
			{
				Id = 1,
				Nome = "Iphone",
				Preco = 149.99m,
				Descricao = "Celular",
				Estoque = 30,
				CategoriaId = 2
			};

			_gestaoProdutos.SalvarProduto(produto);

			_mockRepositorio.Verify(repo => repo.Editar(produto), Times.Once);
		}

		[TestMethod]
		public void ExcluirProduto_test()
		{
			var produto = new Produto
			{
				Id = 1,
				Nome = "Iphone",
				Preco = 149.99m,
				Descricao = "Celular",
				Estoque = 30,
				CategoriaId = 2
			};

			_gestaoProdutos.ExcluirProduto(produto);

			_mockRepositorio.Verify(repo => repo.Excluir(produto), Times.Once);
		}

		[TestMethod]
		public async Task ObterProdutoPor_test()
		{
			Expression<Func<Produto, bool>> predicado = p => p.Id == 1;
			var produtoSimulado = new Produto
			{
				Id = 1,
				Nome = "Iphone",
				Preco = 149.99m,
				Descricao = "Celular",
				Estoque = 30,
				CategoriaId = 2
			};
			_mockRepositorio.Setup(repo => repo.BuscarPor(predicado)).ReturnsAsync(produtoSimulado);

			var resultado = await _gestaoProdutos.BuscarProdutoPor(predicado);

			Assert.IsNotNull(resultado);
			Assert.AreEqual(1, resultado.Id);
			Assert.AreEqual("Iphone", resultado.Nome);
		}

		[TestMethod]
		public async Task ObterProdutos_test()
		{
			var listaProdutosSimulados = new List<Produto>
			{
				new Produto
				{
					Id = 1,
					Nome = "Iphone",
					Preco = 149.99m,
					Descricao = "Celular",
					Estoque = 30,
					CategoriaId = 2
				},
				new Produto
				{
					Id = 0,
					Nome = "Televisao",
					Preco = 300,
					Descricao = "TV LG",
					Estoque = 50,
					CategoriaId = 1
				}
		};
			_mockRepositorio.Setup(repo => repo.ObterTodos()).ReturnsAsync(listaProdutosSimulados);

			var resultado = await _gestaoProdutos.ObterProdutos();

			Assert.IsNotNull(resultado);
			Assert.AreEqual(2, resultado.Count);
		}
	}
}
