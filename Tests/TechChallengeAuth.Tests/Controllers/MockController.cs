using MediatR;
using TechChallengeAuth.Controllers;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace TechChallengeAuth.Tests.Controllers
{
    public class MockController : ControllerBase
    {
        public MockController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
        }

        public new bool OperacaoValida()
        {
            return base.OperacaoValida();
        }

        public new IEnumerable<string> ObterMensagensErro()
        {
            return base.ObterMensagensErro();
        }

        public new void NotificarErro(string codigo, string mensagem)
        {
            base.NotificarErro(codigo, mensagem);
        }

        public new Guid ObterClienteId()
        {
            return base.ObterClienteId();
        }
    }
}
