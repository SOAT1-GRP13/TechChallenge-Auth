using Domain.Base.Messages;
using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Commands.Validation;

namespace Application.Autenticacao.Commands
{
    public class AutenticaClientePorNomeCommand : Command<AutenticaClienteOutput>
    {
        public AutenticaClientePorNomeCommand(AutenticaClientePorNomeInput input)
        {
            Input = input;
        }

        public AutenticaClientePorNomeInput Input { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AutenticaClientePorNomeValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}
