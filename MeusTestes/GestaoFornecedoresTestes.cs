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
	public class GestaoFornecedoresTestes
	{
		private GestaoFornecedores _gestaoFornecedores;
		private Mock<IGenericRepository<Fornecedor>> _mockRepositorio;

		[TestInitialize]
		public void PrepararTeste()
		{
			_mockRepositorio = new Mock<IGenericRepository<Fornecedor>>();
			_gestaoFornecedores = new GestaoFornecedores(_mockRepositorio.Object);
		}

		[TestMethod]
		public void SalvarFornecedor_NovoFornecedor_test()
		{
			var fornecedor = new Fornecedor
			{
				Id = 0,
				CNPJ = "12345678901234",
				RazaoSocial = "Razao Social",
				Telefone = "(11) 98765-4321",
				NomeFantasia = "Nome Fantasia",
				Rua = "Rua A",
				Bairro = "Bairro A",
				Cep = "12345-678",
				Numeracao = 100,
				Cidade = "Cidade A"
			};

			_gestaoFornecedores.SalvarFornecedor(fornecedor);

			_mockRepositorio.Verify(repo => repo.Adicionar(fornecedor), Times.Once);
		}

		[TestMethod]
		public void SalvarFornecedor_FornecedorExistente_test()
		{
			var fornecedor = new Fornecedor
			{
				Id = 0,
				CNPJ = "12345678901234",
				RazaoSocial = "Razao Social",
				Telefone = "(11) 98765-4321",
				NomeFantasia = "Nome Fantasia",
				Rua = "Rua A",
				Bairro = "Bairro A",
				Cep = "12345-678",
				Numeracao = 100,
				Cidade = "Cidade A"
			};

			_gestaoFornecedores.SalvarFornecedor(fornecedor);

			_mockRepositorio.Verify(repo => repo.Editar(fornecedor), Times.Exactly(0));
		}

		[TestMethod]
		public void RemoverFornecedor_test()
		{
			var fornecedor = new Fornecedor
			{
				Id = 0,
				CNPJ = "12345678901234",
				RazaoSocial = "Razao Social",
				Telefone = "(11) 98765-4321",
				NomeFantasia = "Nome Fantasia",
				Rua = "Rua A",
				Bairro = "Bairro A",
				Cep = "12345-678",
				Numeracao = 100,
				Cidade = "Cidade A"
			};

			_gestaoFornecedores.RemoverFornecedor(fornecedor);

			_mockRepositorio.Verify(repo => repo.Excluir(fornecedor), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Once);
		}

		[TestMethod]
		public async Task ObterFornecedorPor_test()
		{
			Expression<Func<Fornecedor, bool>> predicado = f => f.Id == 1;
			var fornecedorSimulado = new Fornecedor
			{
				Id = 1,
				CNPJ = "12345678901234",
				RazaoSocial = "Razao Social",
				Telefone = "(11) 98765-4321",
				NomeFantasia = "Nome Fantasia",
				Rua = "Rua A",
				Bairro = "Bairro A",
				Cep = "12345-678",
				Numeracao = 100,
				Cidade = "Cidade A"
			};
			_mockRepositorio.Setup(repo => repo.BuscarPor(predicado)).ReturnsAsync(fornecedorSimulado);

			var resultado = await _gestaoFornecedores.ObterFornecedorPor(predicado);

			Assert.IsNotNull(resultado);
			Assert.AreEqual(1, resultado.Id);
			Assert.AreEqual("Razao Social", resultado.RazaoSocial);
		}

		[TestMethod]
		public async Task ObterFornecedores_test()
		{
			var listaFornecedoresSimulados = new List<Fornecedor>
			{
				new Fornecedor {Id = 0, CNPJ = "12345678901234", RazaoSocial = "Razao Social", Telefone = "(11) 98765-4321", NomeFantasia = "Nome Fantasia", Rua = "Rua A", Bairro = "Bairro A", Cep = "12345-678", Numeracao = 100, Cidade = "Cidade A"},
				new Fornecedor
				{
					Id = 0,
					CNPJ = "98765432109876",
					RazaoSocial = "Empresa XYZ",
					Telefone = "(22) 5555-4444",
					NomeFantasia = "XYZ LTDA",
					Rua = "Avenida Principal",
					Bairro = "Centro",
					Cep = "54321-987",
					Numeracao = 200,
					Cidade = "Cidade B"
				}
			};
			_mockRepositorio.Setup(repo => repo.ObterTodos()).ReturnsAsync(listaFornecedoresSimulados);

			var resultado = await _gestaoFornecedores.ObterFornecedores();

			Assert.IsNotNull(resultado);
			Assert.AreEqual(2, resultado.Count);
		}
	}
}