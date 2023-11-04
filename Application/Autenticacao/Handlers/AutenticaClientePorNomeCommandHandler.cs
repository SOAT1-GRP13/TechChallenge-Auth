using MediatR;
using Domain.Base.DomainObjects;
using Application.Autenticacao.Commands;
using Application.Autenticacao.UseCases;
using Domain.Base.Communication.Mediator;
using Application.Autenticacao.Boundaries.Cliente;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Autenticacao.Handlers
{
    public class AutenticaClientePorNomeCommandHandler :
        IRequestHandler<AutenticaClientePorNomeCommand, AutenticaClienteOutput>
    {
        private readonly IAutenticacaoUseCase _autenticacaoUseCase;
        private readonly IMediatorHandler _mediatorHandler;
        public AutenticaClientePorNomeCommandHandler(IAutenticacaoUseCase autenticacaoUseCase, IMediatorHandler mediatorHandler)
        {
            _autenticacaoUseCase = autenticacaoUseCase;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<AutenticaClienteOutput> Handle(AutenticaClientePorNomeCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                }

                return new AutenticaClienteOutput();
            }

            try
            {
                return await _autenticacaoUseCase.AutenticaClientePorNome(request.Input.Nome);
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
            }

            return new AutenticaClienteOutput();
        }
    }
}
