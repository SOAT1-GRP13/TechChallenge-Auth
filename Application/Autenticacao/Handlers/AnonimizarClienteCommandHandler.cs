using MediatR;
using Application.Autenticacao.Queries;
using Application.Autenticacao.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Autenticacao.Handlers
{
    public class AnonimizarClienteCommandHandler :
        IRequestHandler<AnonimizarClienteCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAutenticacaoQuery _autenticacaoQuery;

        public AnonimizarClienteCommandHandler(IMediatorHandler mediatorHandler, IAutenticacaoQuery autenticacaoQuery)
        {
            _mediatorHandler = mediatorHandler;
            _autenticacaoQuery = autenticacaoQuery;
        }

        public async Task<bool> Handle(AnonimizarClienteCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                try
                {
                    await _autenticacaoQuery.AnonimizaCliente(request.Input.CPF);
                    return true;
                }
                catch (Exception ex)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
                }
            }
            else
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                }
            }

            return false;
        }
    }
}
