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
	public class GestaoFuncionariosTestes
	{
		private GestaoFuncionarios _gestaoFuncionarios;
		private Mock<IGenericRepository<Funcionario>> _mockRepositorio;

		[TestInitialize]
		public void PrepararTeste()
		{
			_mockRepositorio = new Mock<IGenericRepository<Funcionario>>();
			_gestaoFuncionarios = new GestaoFuncionarios(_mockRepositorio.Object);
		}

		[TestMethod]
		public void SalvarFuncionario_NovoFuncionario_test()
		{
			var funcionario = new Funcionario
			{
				Id = 0,
				Nome = "João Silva",
				Senha = "senha123",
				CPF = "123.456.789-00",
				Telefone = "(11) 98765-4321",
				Rua = "Rua A",
				Bairro = "Centro",
				Cep = "12345-678",
				Numeracao = 100,
				Cidade = "São Paulo"
			};

			_gestaoFuncionarios.SalvarFuncionario(funcionario);

			_mockRepositorio.Verify(repo => repo.Adicionar(funcionario), Times.Once);
		}

		[TestMethod]
		public void SalvarFuncionario_FuncionarioExistente_test()
		{
			var funcionario = new Funcionario
			{
				Id = 1,
				Nome = "Maria Souza",
				Senha = "senha456",
				CPF = "987.654.321-00",
				Telefone = "(11) 12345-6789",
				Rua = "Rua B",
				Bairro = "Centro",
				Cep = "54321-098",
				Numeracao = 200,
				Cidade = "Rio de Janeiro"
			};

			_gestaoFuncionarios.SalvarFuncionario(funcionario);

			_mockRepositorio.Verify(repo => repo.Editar(funcionario), Times.Once);
		}

		[TestMethod]
		public void RemoverFuncionario_test()
		{
			var funcionario = new Funcionario
			{
				Id = 1,
				Nome = "Pedro Santos",
				Senha = "senha789",
				CPF = "111.222.333-00",
				Telefone = "(11) 54321-9876",
				Rua = "Rua C",
				Bairro = "Bairro",
				Cep = "09876-543",
				Numeracao = 300,
				Cidade = "Curitiba"
			};

			_gestaoFuncionarios.RemoverFuncionario(funcionario);

			_mockRepositorio.Verify(repo => repo.Excluir(funcionario), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Once);
		}

		[TestMethod]
		public async Task ObterFuncionarioPor_test()
		{
			Expression<Func<Funcionario, bool>> predicado = f => f.Id == 1;
			var funcionarioSimulado = new Funcionario { Id = 1, Nome = "Funcionario Simulado" };
			_mockRepositorio.Setup(repo => repo.BuscarPor(predicado)).ReturnsAsync(funcionarioSimulado);

			var resultado = await _gestaoFuncionarios.BuscarFuncionarioPor(predicado);

			Assert.IsNotNull(resultado);
			Assert.AreEqual(1, resultado.Id);
			Assert.AreEqual("Funcionario Simulado", resultado.Nome);
		}

		[TestMethod]
		public async Task ObterFuncionarios_test()
		{
			var listaFuncionariosSimulados = new List<Funcionario>
			{
				new Funcionario { Id = 1, Nome = "Funcionario 1" },
				new Funcionario { Id = 2, Nome = "Funcionario 2" }
			};
			_mockRepositorio.Setup(repo => repo.ObterTodos()).ReturnsAsync(listaFuncionariosSimulados);

			var resultado = await _gestaoFuncionarios.ObterFuncionarios();

			Assert.IsNotNull(resultado);
			Assert.AreEqual(2, resultado.Count);
		}
	}
}
