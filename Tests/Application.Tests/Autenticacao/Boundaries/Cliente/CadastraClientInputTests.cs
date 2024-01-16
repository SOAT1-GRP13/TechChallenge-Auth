using Application.Autenticacao.Boundaries.Cliente;

namespace Application.Tests.Autenticacao.Boundaries.Cliente
{
    public class CadastraClienteInputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var cadastraClienteInput = new CadastraClienteInput("63852797071", "teste@123", "teste@teste.com.br", "teste");

            // Act & Assert
            Assert.Equal("63852797071", cadastraClienteInput.CPF);
            Assert.Equal("teste@123", cadastraClienteInput.Senha);
            Assert.Equal("teste@teste.com.br", cadastraClienteInput.Email);
            Assert.Equal("teste", cadastraClienteInput.Nome);
        }
    }
}