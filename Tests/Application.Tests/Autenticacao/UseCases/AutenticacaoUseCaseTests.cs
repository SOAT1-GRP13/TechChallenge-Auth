using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Dto;
using Application.Autenticacao.Dto.Cliente;
using Application.Autenticacao.UseCases;
using Application.Tests.Autenticacao.Mock;
using Domain.Autenticacao;
using Domain.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Autenticacao.UseCases
{
    public class AutenticacaoUseCaseTests : IDisposable
    {

        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly IUsuarioLogadoRepository _usuarioLogadoRepository;
        private readonly IOptions<Secrets> _options;
        private readonly Mock<IAutenticacaoUseCase> _useCaseMock;
        private readonly AutenticacaoUseCase _useCase;

        public AutenticacaoUseCaseTests()
        {
            _autenticacaoRepository = MockAutenticacaoRepository.GetAutenticacaoRepository().Object;
            _usuarioLogadoRepository = MockUsuarioLogadoRepository.GetUsuarioLogadoRepository().Object;
            _options = Options.Create(new Secrets()
            {
                ClientSecret = "9%&ujkio7&*(2)@pwqdmndd[lcnslw$01]{&!jd8#}kfgjaprmb2^40+djl=%-hAiO_$u38",
                PreSalt = "qsdc76543$%4&*(kjhgftyumnb#~;",
                PosSalt = "+_)(rtyumlp;~1'123$#@!GYUJN*&"
            });
            _useCaseMock = new Mock<IAutenticacaoUseCase>();
            _useCase = new AutenticacaoUseCase(_autenticacaoRepository, _usuarioLogadoRepository, _options);
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
            _autenticacaoRepository.Dispose();
            _useCase.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}