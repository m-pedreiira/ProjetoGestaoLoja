using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetosTresCamadas.Data.GenericRepository;
using ProjetosTresCamadas.Data.Reposity;
using ProjetoTresCamadas.Bussines.Services;
using ProjetoTresCamadas.DTO.Entidades;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MeusTestes.Testes.Services
{
	[TestClass]
	public class GestaoClientesTestes
	{
		private GestaoClientes _gestaoClientes;
		private Mock<IGenericRepository<Cliente>> _mockRepositorio;

		[TestInitialize]
		public void PrepararTeste()
		{
			_mockRepositorio = new Mock<IGenericRepository<Cliente>>();
			_gestaoClientes = new GestaoClientes(_mockRepositorio.Object);
		}

		[TestMethod]
		public void SalvarCliente_NovoCliente_test()
		{
			
			var cliente = new Cliente
			{
				Id = 0,
				Nome = "Mateus Pedreira",
				CPF = "111.222.333-44",
				Telefone = "(11) 11111-1111",
				Rua = "Rua A",
				Bairro = "São Gabriel",
				Cep = "11111-111",
				Numeracao = 101,
				Cidade = "Belo Horizonte"

			};


			var resultado = _gestaoClientes.SalvarCliente(cliente);

			Assert.AreEqual("Cliente salvo no banco de dados com Sucesso", resultado);
			_mockRepositorio.Verify(repo => repo.Adicionar(cliente), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Once);
			_mockRepositorio.Verify(repo => repo.Dispose(), Times.Once);
		}


		[TestMethod]
		public async Task ObterClientes_test()
		{
			var clientesSimulados = new List<Cliente>
			{
				new Cliente
				{
					Id = 1,
					Nome = "Mateus Pedreira",
					CPF = "111.222.333-44",
					Telefone = "(11) 11111-1111",
					Rua = "Rua A",
					Bairro = "São Gabriel",
					Cep = "11111-111",
					Numeracao = 101,
					Cidade = "Belo Horizonte"

				},
				new Cliente { Id = 2, Nome = "Thiago soares" }
			};
			_mockRepositorio.Setup(repo => repo.ObterTodos()).ReturnsAsync(clientesSimulados);

			var resultado = await _gestaoClientes.ObterClientes();

			Assert.IsNotNull(resultado);
			Assert.AreEqual(2, resultado.Count);
		}

		[TestMethod]
		public async Task BuscarCliente_test()
		{
			Expression<Func<Cliente, bool>> atributo = c => c.Id == 1;
			var clienteSimulado = new Cliente { Id = 1, Nome = "Mateus Pedreira" };
			_mockRepositorio.Setup(repo => repo.BuscarPor(atributo)).ReturnsAsync(clienteSimulado);

			var resultado = await _gestaoClientes.BuscarCliente(atributo);

			Assert.IsNotNull(resultado);
			Assert.AreEqual(1, resultado.Id);
			Assert.AreEqual("Mateus Pedreira", resultado.Nome);
		}

		[TestMethod]
		public void SalvarCliente_ClienteExistente_test()
		{
			var cliente = new Cliente { Id = 1, Nome = "Cliente Existente" };

			var resultado = _gestaoClientes.SalvarCliente(cliente);

			Assert.AreEqual("A edicão do cliente no banco de dados foi feita com sucesso!", resultado);
			_mockRepositorio.Verify(repo => repo.Editar(cliente), Times.Once);
			_mockRepositorio.Verify(repo => repo.Salvar(), Times.Once);
			_mockRepositorio.Verify(repo => repo.Dispose(), Times.Once);
		}

		[TestMethod]
		public void RemoverCliente_test()
		{
			var cliente = new Cliente { Id = 1, Nome = "Cliente Existente" };

			var resultado = _gestaoClientes.RemoverCliente(cliente);

			Assert.AreEqual("Cliente removido do banco de dados com sucesso!", resultado);
			_mockRepositorio.Verify(repo => repo.Excluir(cliente), Times.Once);
		}

		[TestMethod]
		public void Dispose()
		{
			_gestaoClientes.Dispose();

			_mockRepositorio.Verify(repo => repo.Dispose(), Times.Once);
		}

		[TestMethod]
		public void Dispose_test()
		{
			_gestaoClientes.Dispose();

			_mockRepositorio.Verify(repo => repo.Dispose(), Times.Once);
		}
	}
}