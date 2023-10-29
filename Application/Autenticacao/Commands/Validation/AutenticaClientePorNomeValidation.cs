using FluentValidation;
using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Autenticacao.Commands.Validation
{
    public class AutenticaClientePorNomeValidation : AbstractValidator<AutenticaClientePorNomeInput>
    {
        public AutenticaClientePorNomeValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");
        }
    }
}
