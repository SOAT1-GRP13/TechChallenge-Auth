
using Application.Autenticacao.Boundaries.LogIn;

namespace Application.Tests.Autenticacao.Boundaries.LogIn
{
    public class LogInOutputTests
    {
        [Fact]
        public void AtribuirEObterValores_DeveFuncionarCorretamente()
        {
            // Arrange
            var logInUsuarioInput = new LogInUsuarioOutput("teste", "tokenacesso");

            // Act & Assert
            Assert.Equal("teste", logInUsuarioInput.NomeUsuario);
            Assert.Equal("tokenacesso", logInUsuarioInput.TokenAcesso);
        }
    }
}