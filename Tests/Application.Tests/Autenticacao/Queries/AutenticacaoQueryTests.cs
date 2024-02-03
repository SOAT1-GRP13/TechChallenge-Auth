using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Dto.Cliente;
using Application.Autenticacao.Queries;
using Application.Tests.Autenticacao.Mock.Repositories;
using Domain.Autenticacao;
using Domain.Base.Data;
using Moq;

namespace Application.Tests.Autenticacao.Queries
{
    public class AutenticacaoQueryTests : IDisposable
    {

        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly Mock<IAutenticacaoQuery> _queryMock;
        private readonly AutenticacaoQuery _query;

        public AutenticacaoQueryTests()
        {
            _autenticacaoRepository = MockAutenticacaoRepository.GetAutenticacaoRepository().Object;

            _queryMock = new Mock<IAutenticacaoQuery>();
            _query = new AutenticacaoQuery(_autenticacaoRepository);
        }

        [Fact]
        public async Task CadastraClienteTest()
        {
            var cliente = new CadastraClienteDto(
                    "teste@123",
                    new CadastraClienteInput(
                    "63852797071",
                    "teste@123",
                    "teste@teste.com.br",
                    "teste")

                    );

            _queryMock.Setup(u => u.CadastraCliente(cliente));

            await _query.CadastraCliente(cliente);
        }

        [Fact]
        public async Task ClienteJaExisteTest()
        {
            var cliente = new CadastraClienteDto(
                "teste@123",
                new CadastraClienteInput(
                "63852797071",
                "teste@123",
                "teste@teste.com.br",
                "teste")
                );

            _queryMock.Setup(u => u.ClienteJaExiste(cliente)).ReturnsAsync(true);

            var resultado = await _query.ClienteJaExiste(cliente);

            //Assert
            Assert.True(resultado);
        }

        public void Dispose()
        {
            _autenticacaoRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}