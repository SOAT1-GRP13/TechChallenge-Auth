
using Application.Autenticacao.Boundaries.LogIn;

namespace Application.Tests.Autenticacao.Boundaries.LogIn
{
    public class LogInInputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var logInUsuarioInput = new LogInUsuarioInput("teste", "teste@123");

            // Act & Assert
            Assert.Equal("teste", logInUsuarioInput.NomeUsuario);
            Assert.Equal("teste@123", logInUsuarioInput.Senha);
        }
    }
}