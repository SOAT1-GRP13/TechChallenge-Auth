using API.Tests;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechChallengeAuth.Controllers;

namespace TechChallengeAuth.Tests.Controllers
{
    public class AdminControllerTestes : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceProvider _serviceProvideStartTup;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;

        public AdminControllerTestes()
        {
            _serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            _serviceProvideStartTup = new TestStartup().ConfigureServices(new ServiceCollection());
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
        }

        #region Testes unitários do LogInUsuario
        [Fact]
        public async Task AoChamarLoginUsuario_DeveRetornarOK_QuandoAsCredenciasEstiveremCorretas()
        {

            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var adminController = new AdminController(domainNotificationHandler, _mediatorHandlerMock.Object);

            var loginInput = new LogInUsuarioInput(
             "fiap",
             "Teste@123"
            );

            _mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            _mediatorHandlerMock.Setup(x => x.EnviarComando<AdminAutenticaCommand, LogInUsuarioOutput>(It.IsAny<AdminAutenticaCommand>()))
                .ReturnsAsync(new LogInUsuarioOutput(loginInput.NomeUsuario, "fdhfjsdhfjksdhfkj"));

            var resultado = await adminController.LogInUsuario(loginInput);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(resultado);
            var loginRetornado = Assert.IsType<LogInUsuarioOutput>(objectResult.Value);
            Assert.Equal(loginInput.NomeUsuario, loginRetornado.NomeUsuario);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task AoChamarLoginUsuario_DeveRetornarBadRequest_AoNaoPreencherOsCamposObrigatorios()
        {
            // Arrange
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var adminController = new AdminController(domainNotificationHandler, mediatorHandler);

            var loginInput = new LogInUsuarioInput();

            //Act
            var resultado = await adminController.LogInUsuario(loginInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("Nome do usuário é obrigatório", mensagensErro);
            Assert.Contains("Senha é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task AoChamarLoginUsuario_DeveRetornarBadRequest_AoPreencherCredenciaisInvalidas()
        {
            // Arrange

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var adminController = new AdminController(domainNotificationHandler, _mediatorHandlerMock.Object);

            var loginInput = new LogInUsuarioInput("usuarioInvalido", "senhaInvalida");

            _mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            domainNotificationHandler.Handle(new DomainNotification("Erro", "Usuário ou senha inválidos"), CancellationToken.None).Wait();

            _mediatorHandlerMock.Setup(x => x.EnviarComando<AdminAutenticaCommand, LogInUsuarioOutput>(It.IsAny<AdminAutenticaCommand>()))
                .ReturnsAsync(new LogInUsuarioOutput("", ""));

            var controller = new MockController(domainNotificationHandler, _mediatorHandlerMock.Object);

            //Act
            var resultado = await adminController.LogInUsuario(loginInput);
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("Usuário ou senha inválidos", mensagensErro);
        }

        [Fact]
        public async Task AoChamarLoginUsuaro_DeveRetornarInternalError_AoOcorrerErroInesperado()
        {
            // Arrange
            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();

            var adminController = new AdminController(domainNotificationHandler, _mediatorHandlerMock.Object);

            var loginInput = new LogInUsuarioInput("usuarioInvalido", "senhaInvalida");

            _mediatorHandlerMock.Setup(x => x.EnviarComando<AdminAutenticaCommand, LogInUsuarioOutput>(It.IsAny<AdminAutenticaCommand>()))
                .ThrowsAsync(new Exception("Simulando uma exceção"));

            var resultado = await adminController.LogInUsuario(loginInput);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Erro ao tentar realizar LogIn. Erro: Simulando uma exceção", objectResult.Value);
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}