using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Dto;
using Application.Autenticacao.Dto.Cliente;
using Application.Autenticacao.UseCases;
using Application.Tests.Autenticacao.Mock.UseCases;
using Moq;

namespace Application.Tests.Autenticacao.UseCases
{
    public class AutenticacaoUseCaseTests : IDisposable
    {
        private readonly Mock<IAutenticacaoUseCase> _useCaseMock;
        private readonly AutenticacaoUseCase _useCase;

        public AutenticacaoUseCaseTests()
        {

            _useCaseMock = MockAutenticacaoUseCase.GetAutencacaoUseCaseMock();
            _useCase = MockAutenticacaoUseCase.GetAutenticacaoUseCase();
        }

        [Fact]
        public async Task AutenticaUsuarioTest()
        {
            LoginUsuarioDto input = new LoginUsuarioDto("fiapUser", "Teste@123");

            _useCaseMock.Setup(u => u.AutenticaUsuario(input)).ReturnsAsync(new LogInUsuarioOutput("fiapUser", "tokenAcesso"));

            var resultado = await _useCase.AutenticaUsuario(input);

            //Assert
            var loginRetornado = Assert.IsType<LogInUsuarioOutput>(resultado);
            Assert.Equal("fiapUser", loginRetornado.NomeUsuario);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        [Fact]
        public async Task AutenticaClienteTest()
        {
            var input = new IdentificaDto("63852797071", "Teste@123");

            _useCaseMock.Setup(u => u.AutenticaCliente(input)).ReturnsAsync(new AutenticaClienteOutput("teste", "tokenAcesso"));

            var resultado = await _useCase.AutenticaCliente(input);

            //Assert
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(resultado);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        [Fact]
        public async Task AutenticaClientePorNomeTest()
        {
            _useCaseMock.Setup(u => u.AutenticaClientePorNome("teste")).ReturnsAsync(new AutenticaClienteOutput("teste", "tokenAcesso"));

            var resultado = await _useCase.AutenticaClientePorNome("teste");

            //Assert
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(resultado);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        public void Dispose()
        {
            _useCase.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}