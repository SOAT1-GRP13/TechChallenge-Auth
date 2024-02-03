using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace TechChallengeAuth.Tests.Controllers
{
    public class ControllerBaseTests
    {
        [Fact]
        public void OperacaoValida_DeveRetornarFalso_QuandoExistemNotificacoes()
        {
            // Arrange
            var notifications = new DomainNotificationHandler();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            notifications.Handle(new DomainNotification("Erro", "Mensagem de erro"), CancellationToken.None).Wait();
            var controller = new MockController(notifications, mediatorHandlerMock.Object); // MockController é uma implementação concreta de ControllerBase

            // Act
            var operacaoValida = controller.OperacaoValida();
            var mensagensErro = controller.ObterMensagensErro();

            // Assert
            Assert.False(operacaoValida);
            Assert.Contains("Mensagem de erro", mensagensErro);
        }

        [Fact]
        public void NotificarErro_DevePublicarNotificacao()
        {
            // Arrange
            var mediatorMock = new Mock<IMediatorHandler>();
            var notifications = new DomainNotificationHandler();
            var controller = new MockController(notifications, mediatorMock.Object);

            // Act
            controller.NotificarErro("Erro", "Mensagem de erro");

            // Assert
            mediatorMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        }

        [Fact]
        public void ObterClienteId_DeveRetornarGuid_QuandoUsuarioAutenticado()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            var controller = new MockController(new DomainNotificationHandler(), mediatorHandlerMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = controller.ObterClienteId();

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public void ObterClienteId_DeveLancarExcecao_QuandoUsuarioNaoAutenticado()
        {
            // Arrange
            var principal = new ClaimsPrincipal();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            var controller = new MockController(new DomainNotificationHandler(), mediatorHandlerMock.Object)
            {
                ControllerContext = controllerContext
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => controller.ObterClienteId());
            Assert.Equal("Cliente não identificado", exception.Message);
        }

    }
}
