using Swashbuckle.AspNetCore.Annotations;

namespace Application.Autenticacao.Boundaries.Cliente
{
    public class AnonimizarClienteInput
    {
        public AnonimizarClienteInput()
        {
            CPF = string.Empty;
        }
        public AnonimizarClienteInput(string cPF)
        {
            CPF = cPF;
        }

        [SwaggerSchema(
            Title = "CPF",
            Description = "Preencha com um CPF válido",
            Format = "string")]
        public string CPF { get; set; }
    }
}
