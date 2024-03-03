using API.Tests;
using Application.Autenticacao.Boundaries.Cliente;
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
    public class ClienteControllerTestes : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceProvider _serviceProvideStartTup;

        public ClienteControllerTestes()
        {
            _serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            _serviceProvideStartTup = new TestStartup().ConfigureServices(new ServiceCollection());
        }

        #region Testes unitários do AutenticaCliente
        [Fact]
        public async Task AoChamarAutenticaCliente_DeveRetornarOK_QuandoAsCredenciasEstiveremCorretas()
        {

            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var loginInput = new AutenticaClienteInput(
             "47253197836",
             "Teste@123"
            );

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            mediatorHandlerMock.Setup(x => x.EnviarComando<AutenticaClienteCommand, AutenticaClienteOutput>(It.IsAny<AutenticaClienteCommand>()))
                .ReturnsAsync(new AutenticaClienteOutput("teste", "fdhfjsdhfjksdhfkj"));

            var resultado = await clienteController.AutenticaCliente(loginInput);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(resultado);
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(objectResult.Value);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task AoChamarAutenticaCliente_DeveRetornarBadRequest_AoNaoPreencherOsCamposObrigatorios()
        {
            // Arrange

            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandler);

            var loginInput = new AutenticaClienteInput();

            //Act
            var resultado = await clienteController.AutenticaCliente(loginInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("CPF é obrigatório", mensagensErro);
            Assert.Contains("Senha é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task AoChamarAutenticaCliente_DeveRetornarBadRequest_AoPreencherCredenciaisInvalidas()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var loginInput = new AutenticaClienteInput("usuarioInvalido", "senhaInvalida");

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            domainNotificationHandler.Handle(new DomainNotification("Erro", "CPF ou senha inválidos"), CancellationToken.None).Wait();

            mediatorHandlerMock.Setup(x => x.EnviarComando<AutenticaClienteCommand, AutenticaClienteOutput>(It.IsAny<AutenticaClienteCommand>()))
                .ReturnsAsync(new AutenticaClienteOutput("", ""));

            var controller = new MockController(domainNotificationHandler, mediatorHandlerMock.Object);



            //Act
            var resultado = await clienteController.AutenticaCliente(loginInput);
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("CPF ou senha inválidos", mensagensErro);
        }

        [Fact]
        public async Task AoChamarAutenticaCliente_DeveRetornarInternalError_AoOcorrerErroInesperado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var loginInput = new AutenticaClienteInput("usuarioInvalido", "senhaInvalida");

            mediatorHandlerMock.Setup(x => x.EnviarComando<AutenticaClienteCommand, AutenticaClienteOutput>(It.IsAny<AutenticaClienteCommand>()))
                .ThrowsAsync(new Exception("Simulando uma exceção"));

            var resultado = await clienteController.AutenticaCliente(loginInput);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Erro ao tentar realizar LogIn. Erro: Simulando uma exceção", objectResult.Value);
        }

        #endregion

        #region Teste relecionados ao AutenticaCliente por nome

        [Fact]
        public async Task AoChamarAutenticaClientePorNome_DeveRetornarOk_AoInformarUmNome()
        {
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var loginInput = new AutenticaClientePorNomeInput(
             "teste"
            );

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            mediatorHandlerMock.Setup(x => x.EnviarComando<AutenticaClientePorNomeCommand, AutenticaClienteOutput>(It.IsAny<AutenticaClientePorNomeCommand>()))
                .ReturnsAsync(new AutenticaClienteOutput("teste", "fdhfjsdhfjksdhfkj"));

            var resultado = await clienteController.AutenticaClientePorNome(loginInput);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(resultado);
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(objectResult.Value);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task AoChamarAutenticaClientePorNome_DeveRetornarBadRequest_AoNaoPreencherOsCamposObrigatorios()
        {
            // Arrange
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandler);

            var loginInput = new AutenticaClientePorNomeInput();

            //Act
            var resultado = await clienteController.AutenticaClientePorNome(loginInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("Nome é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task AoChamarAutenticaClientePorNome_DeveRetornarInternalError_AoOcorrerErroInesperado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var loginInput = new AutenticaClientePorNomeInput("usuarioInvalido");

            mediatorHandlerMock.Setup(x => x.EnviarComando<AutenticaClientePorNomeCommand, AutenticaClienteOutput>(It.IsAny<AutenticaClientePorNomeCommand>()))
                .ThrowsAsync(new Exception("Simulando uma exceção"));

            var resultado = await clienteController.AutenticaClientePorNome(loginInput);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Erro ao tentar realizar LogIn. Erro: Simulando uma exceção", objectResult.Value);
        }
        #endregion

        #region Testes relacionados ao cadastra cliente

        [Fact]
        public async Task AoChamarCadastraCliente_DeveAutenticar_E_RetornarOk_AoPreencherUmClienteValido()
        {
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var cadastraInput = new CadastraClienteInput(
             "47253197836",
             "Teste@123",
             "teste@teste.com.br",
             "teste"
            );

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            mediatorHandlerMock.Setup(x => x.EnviarComando<CadastraClienteCommand, AutenticaClienteOutput>(It.IsAny<CadastraClienteCommand>()))
                .ReturnsAsync(new AutenticaClienteOutput("teste", "fdhfjsdhfjksdhfkj"));

            var resultado = await clienteController.CadastraCliente(cadastraInput);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(resultado);
            var loginRetornado = Assert.IsType<AutenticaClienteOutput>(objectResult.Value);
            Assert.Equal("teste", loginRetornado.Nome);
            Assert.True(!string.IsNullOrEmpty(loginRetornado.TokenAcesso));

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task AoChamarCadastraCliente_DeveRetornarBadRequest_AoNaoPreencherOsCamposObrigatorios()
        {
            // Arrange
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandler);

            var cadastraInput = new CadastraClienteInput();

            //Act
            var resultado = await clienteController.CadastraCliente(cadastraInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("Nome é obrigatório", mensagensErro);
            Assert.Contains("Senha é obrigatória", mensagensErro);
            Assert.Contains("E-mail é obrigatório", mensagensErro);
            Assert.Contains("CPF é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task AoChamarCadastraCliente_DeveRetornarBadRequest_AoPreencherEmailInvalido()
        {
            // Arrange
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandler);

            var cadastraInput = new CadastraClienteInput(
                "47253197836",
                "Teste@123",
                "teste",
                "teste"
                );

            //Act
            var resultado = await clienteController.CadastraCliente(cadastraInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("E-mail inválido", mensagensErro);

        }

        [Fact]
        public async Task AoChamarCadastraCliente_DeveRetornarBadRequest_AoPreencherCPFJaUtilizado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var cadastraInput = new CadastraClienteInput(
                "47253197836",
                "Teste@123",
                "teste@teste.com",
                "teste"
                );

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            domainNotificationHandler.Handle(new DomainNotification("Erro", "Cliente ja cadastrado"), CancellationToken.None).Wait();

            mediatorHandlerMock.Setup(x => x.EnviarComando<CadastraClienteCommand, AutenticaClienteOutput>(It.IsAny<CadastraClienteCommand>()))
                .ReturnsAsync(new AutenticaClienteOutput("", ""));

            var controller = new MockController(domainNotificationHandler, mediatorHandlerMock.Object);



            //Act
            var resultado = await clienteController.CadastraCliente(cadastraInput);
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("Cliente ja cadastrado", mensagensErro);
        }

        [Fact]
        public async Task AoChamarCadastraCliente_DeveRetornarInternalError_AoOcorrerErroInesperado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var cadastraInput = new CadastraClienteInput(
                "47253197836",
                "Teste@123",
                "teste@teste.com",
                "teste"
                );

            mediatorHandlerMock.Setup(x => x.EnviarComando<CadastraClienteCommand, AutenticaClienteOutput>(It.IsAny<CadastraClienteCommand>()))
                .ThrowsAsync(new Exception("Simulando uma exceção"));

            var resultado = await clienteController.CadastraCliente(cadastraInput);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Erro ao tentar cadastrar usuario. Erro: Simulando uma exceção", objectResult.Value);
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Testes relacionados ao AnonimizarCliente
        [Fact]
        public async Task AoChamarAnonimizarCliente_DeveRetornarOk_AoInformarUmClienteValido()
        {
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var anonimizarInput = new AnonimizarClienteInput(
                            "47253197836"
                                       );

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            mediatorHandlerMock.Setup(x => x.EnviarComando<AnonimizarClienteCommand, bool>(It.IsAny<AnonimizarClienteCommand>()))
                .ReturnsAsync(true);

            var resultado = await clienteController.AnonimizarCliente(anonimizarInput);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(resultado);
            var anonimizado = Assert.IsType<bool>(objectResult.Value);
            Assert.True(anonimizado);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task AoChamarAnonimizarCliente_DeveRetornarBadRequest_AoNaoPreencherOsCamposObrigatorios()
        {
            // Arrange
            var domainNotificationHandler = _serviceProvideStartTup.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = _serviceProvideStartTup.GetRequiredService<IMediatorHandler>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandler);

            var anonimizarInput = new AnonimizarClienteInput();

            //Act
            var resultado = await clienteController.AnonimizarCliente(anonimizarInput);

            // Assert
            var badRequestObjectResult = Assert.IsType<ObjectResult>(resultado);
            var mensagensErro = Assert.IsType<List<string>>(badRequestObjectResult.Value);
            Assert.Contains("CPF é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task AoChamarAnonimizarCliente_DeveRetornarInternalError_AoOcorrerErroInesperado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var anonimizarInput = new AnonimizarClienteInput("47253197836");

            mediatorHandlerMock.Setup(x => x.EnviarComando<AnonimizarClienteCommand, bool>(It.IsAny<AnonimizarClienteCommand>()))
                .ThrowsAsync(new Exception("Simulando uma exceção"));

            var resultado = await clienteController.AnonimizarCliente(anonimizarInput);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("Erro ao tentar anonimizar usuário. Erro: Simulando uma exceção", objectResult.Value);
        }

        [Fact]
        public async Task AoChamarAnonimizarCliente_DeveRetornarBadRequest_AoInformarUmClienteInvalido()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var anonimizarInput = new AnonimizarClienteInput("usuarioInvalido");

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            domainNotificationHandler.Handle(new DomainNotification("Erro", "CPF inválido"), CancellationToken.None).Wait();

            mediatorHandlerMock.Setup(x => x.EnviarComando<AnonimizarClienteCommand, bool>(It.IsAny<AnonimizarClienteCommand>()))
                .ReturnsAsync(false);

            var controller = new MockController(domainNotificationHandler, mediatorHandlerMock.Object);

            //Act
            var resultado = await clienteController.AnonimizarCliente(anonimizarInput);
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("CPF inválido", mensagensErro);
        }

        [Fact]
        public async Task AoChamarAnonimizarCliente_DeveRetornarBadRequest_AoInformarUmClienteNaoEncontrado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = _serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var clienteController = new ClienteController(domainNotificationHandler, mediatorHandlerMock.Object);

            var anonimizarInput = new AnonimizarClienteInput("usuarioInvalido");

            mediatorHandlerMock.Setup(x => x.PublicarNotificacao(It.IsAny<DomainNotification>()));

            domainNotificationHandler.Handle(new DomainNotification("Erro", "Cliente não encontrado"), CancellationToken.None).Wait();

            mediatorHandlerMock.Setup(x => x.EnviarComando<AnonimizarClienteCommand, bool>(It.IsAny<AnonimizarClienteCommand>()))
                .ReturnsAsync(false);

            var controller = new MockController(domainNotificationHandler, mediatorHandlerMock.Object);

            //Act
            var resultado = await clienteController.AnonimizarCliente(anonimizarInput);
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("Cliente não encontrado", mensagensErro);
        }
        #endregion
    }
}