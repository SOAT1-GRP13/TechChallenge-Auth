using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Base.Messages;
using MediatR;

namespace Domain.Base.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator) 
        {
            _mediator = mediator;
        }

        public async Task<TResponse> EnviarComando<TCommand, TResponse>(TCommand command) where TCommand : Command<TResponse>
        {
            return await _mediator.Send(command);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }
    }
}
