using Swashbuckle.AspNetCore.Annotations;

namespace Application.Autenticacao.Boundaries.Cliente
{
    public class AutenticaClientePorNomeInput
    {
        public AutenticaClientePorNomeInput(string nome)
        {
            Nome = nome;
        }

        [SwaggerSchema(
            Title = "Nome",
            Description = "Nome do cliente",
            Format = "string")]
        public string Nome { get; set; }
    }
}
