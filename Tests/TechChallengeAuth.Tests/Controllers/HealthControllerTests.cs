using Moq;
using MediatR;
using TechChallengeAuth.Controllers;
using Microsoft.AspNetCore.Mvc;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace TechChallengeAuth.Tests.Controllers
{
    public class HealthControllerTests
    {
        [Fact]
        public void AoChamarHealthCheck_DeveRetornarStatusOk()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            var healthController = new HealthController(
                domainNotificationHandler,
                mediatorHandlerMock.Object
            );

            // Act
            var resultado = healthController.HealthCheck();

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
