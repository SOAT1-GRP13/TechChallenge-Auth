using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Tests.Autenticacao.Boundaries.Cliente
{
    public class AutenticaClienteOutputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var autenticaClienteOutput = new AutenticaClienteOutput("teste", "tokenAcesso");

            // Act & Assert
            Assert.Equal("teste", autenticaClienteOutput.Nome);
            Assert.Equal("tokenAcesso", autenticaClienteOutput.TokenAcesso);
        }
    }
}