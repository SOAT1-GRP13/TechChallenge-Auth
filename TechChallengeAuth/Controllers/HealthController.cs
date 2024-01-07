
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TechChallengeAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Endpoints relacionados a pedidos, sendo necessário se autenticar")]
    public class HealthController : ControllerBase
    {

        public HealthController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Health check",
            Description = "testa se a API está no ar")]
        [SwaggerResponse(200, "Retorna se OK")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
