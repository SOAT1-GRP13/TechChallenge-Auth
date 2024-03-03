using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Commands.Validation;
using Domain.Base.Messages;

namespace Application.Autenticacao.Commands
{
    public class AnonimizarClienteCommand : Command<bool>
    {
        public AnonimizarClienteCommand(AnonimizarClienteInput input) 
        {
            Input = input;
        }

        public AnonimizarClienteInput Input { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AnonimizarClienteValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}
