using FluentValidation;
using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Autenticacao.Commands.Validation
{
    public class AnonimizarClienteValidation : AbstractValidator<AnonimizarClienteInput>
    {
        public AnonimizarClienteValidation()
        {
            RuleFor(a => a.CPF)
                .NotEmpty()
                .WithMessage("CPF é obrigatório");
        }
    }
}
