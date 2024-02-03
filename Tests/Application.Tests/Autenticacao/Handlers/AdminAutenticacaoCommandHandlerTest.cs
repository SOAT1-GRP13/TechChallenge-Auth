using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Commands;
using Application.Autenticacao.Dto;
using Application.Autenticacao.Handlers;
using Application.Autenticacao.UseCases;
using Application.Tests.Autenticacao.Mock.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Moq;

namespace Application.Tests.Autenticacao.Handlers
{
    public class AdminAutenticacaoCommandHandlerTest : IDisposable
    {
        private readonly Mock<IAutenticacaoUseCase> _useCaseMock;
        private readonly AdminAutenticacaoCommandHandler _handler;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;

        public AdminAutenticacaoCommandHandlerTest()
        {
            _useCaseMock = MockAutenticacaoUseCase.GetAutencacaoUseCaseMock();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new AdminAutenticacaoCommandHandler(_useCaseMock.Object, _mediatorHandlerMock.Object);
        }

        [Fact]
        public async Task HandleAutenticaUsuarioTest_Autenticado()
        {
            LogInUsuarioInput input = new LogInUsuarioInput("fiapUser", "Teste@123");

            _useCaseMock.Setup(u => u.AutenticaUsuario(It.IsAny<LoginUsuarioDto>())).ReturnsAsync(new LogInUsuarioOutput("fiapUser", "tokenAcesso"));

            var result = await _handler.Handle(new AdminAutenticaCommand(input), CancellationToken.None);

            //Assert
            var loginRetornado = Assert.IsType<LogInUsuarioOutput>(result);
            Assert.Equal("fiapUser", loginRetornado.NomeUsuario);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        [Fact]
        public async Task HandleAutenticaUsuarioTest_NaoAutenticado()
        {
            LogInUsuarioInput input = new LogInUsuarioInput("usuarioinvalido", "Teste@123invalido");

            _useCaseMock.Setup(u => u.AutenticaUsuario(It.IsAny<LoginUsuarioDto>())).ReturnsAsync(new LogInUsuarioOutput());

            _mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            _mediatorHandlerMock.Object.PublicarNotificacao(new DomainNotification("Erro", "Usuário ou senha inválidos")).Wait();

            var result = await _handler.Handle(new AdminAutenticaCommand(input), CancellationToken.None);

            //Assert
            var loginRetornado = Assert.IsType<LogInUsuarioOutput>(result);
            Assert.True(string.IsNullOrEmpty(loginRetornado.NomeUsuario));
            Assert.True(string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}