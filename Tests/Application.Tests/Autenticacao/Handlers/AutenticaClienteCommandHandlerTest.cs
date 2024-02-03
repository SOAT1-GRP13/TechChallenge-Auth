using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Commands;
using Application.Autenticacao.Dto.Cliente;
using Application.Autenticacao.Handlers;
using Application.Autenticacao.UseCases;
using Application.Tests.Autenticacao.Mock.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Moq;

namespace Application.Tests.Autenticacao.Handlers
{
    public class AutenticaClienteCommandHandlerTest : IDisposable
    {
        private readonly Mock<IAutenticacaoUseCase> _useCaseMock;
        private readonly AutenticaClienteCommandHandler _handler;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;

        public AutenticaClienteCommandHandlerTest()
        {
            _useCaseMock = MockAutenticacaoUseCase.GetAutencacaoUseCaseMock();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new AutenticaClienteCommandHandler(_useCaseMock.Object, _mediatorHandlerMock.Object);
        }

        [Fact]
        public async Task HandleAutenticaUsuarioTest_Autenticado()
        {
            AutenticaClienteInput input = new AutenticaClienteInput("63852797071", "Teste@123");

            _useCaseMock.Setup(u => u.AutenticaCliente(It.IsAny<IdentificaDto>())).ReturnsAsync(new AutenticaClienteOutput("teste", "tokenAcesso"));

            var result = await _handler.Handle(new AutenticaClienteCommand(input), CancellationToken.None);

            //Assert
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(result);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        [Fact]
        public async Task HandleAutenticaUsuarioTest_NaoAutenticado()
        {
            AutenticaClienteInput input = new AutenticaClienteInput("63852797071", "Teste@123");

            _useCaseMock.Setup(u => u.AutenticaCliente(It.IsAny<IdentificaDto>())).ReturnsAsync(new AutenticaClienteOutput());

            _mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            _mediatorHandlerMock.Object.PublicarNotificacao(new DomainNotification("Erro", "CPF ou senha inválidos")).Wait();

            var result = await _handler.Handle(new AutenticaClienteCommand(input), CancellationToken.None);

            //Assert
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(result);
            Assert.True(string.IsNullOrEmpty(loginRetornado.Nome));
            Assert.True(string.IsNullOrEmpty(loginRetornado.TokenAcesso));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}