using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Tests.Autenticacao.Boundaries.Cliente
{
    public class AutenticaClienteInputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var autenticaClienteInput = new AutenticaClienteInput("63852797071", "teste@123");

            // Act & Assert
            Assert.Equal("63852797071", autenticaClienteInput.CPF);
        }
    }
}