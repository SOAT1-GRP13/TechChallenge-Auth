using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Tests.Autenticacao.Boundaries.Cliente
{
    public class AutenticaClientePorNomeInputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var autenticaClientePorNomeInput = new AutenticaClientePorNomeInput("teste");

            // Act & Assert
            Assert.Equal("teste", autenticaClientePorNomeInput.Nome);
        }
    }
}