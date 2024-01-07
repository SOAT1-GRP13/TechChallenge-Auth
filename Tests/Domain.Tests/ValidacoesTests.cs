using Domain.Base.DomainObjects;
using Xunit.Sdk;

namespace Domain.Testes
{
    public class ValidacoesTests
    {
        [Fact]
        public void AoChamarValidarSeIgual_SeObjetosForemIguais_LancarExcecao()
        {
            // Arrange
            var objeto1 = new object();
            var objeto2 = objeto1;
            var mensagem = "Objetos são iguais";

            // Act
            try
            {
                Validacoes.ValidarSeIgual(objeto1, objeto2, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeIgual_SeObjetosForemDiferentes_NaoLancarExcecao()
        {
            // Arrange
            var objeto1 = new object();
            var objeto2 = new object();
            var mensagem = "Objetos são iguais";

            // Act
            try
            {
                Validacoes.ValidarSeIgual(objeto1, objeto2, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeDiferente_SeObjetosForemDiferentes_LancarExcecao()
        {
            // Arrange
            var objeto1 = new object();
            var objeto2 = new object();
            var mensagem = "Objetos são diferentes";

            // Act
            try
            {
                Validacoes.ValidarSeDiferente(objeto1, objeto2, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeDiferente_SeObjetosForemIguais_NaoLancarExcecao()
        {
            // Arrange
            var objeto1 = new object();
            var objeto2 = objeto1;
            var mensagem = "Objetos são diferentes";

            // Act
            try
            {
                Validacoes.ValidarSeDiferente(objeto1, objeto2, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeDiferente_SeStringsForemDiferentes_LancarExcecao()
        {
            // Arrange
            var pattern = "^[a-zA-Z]*$";
            var valor = "123456";
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeDiferente(pattern, valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeDiferente_SeStringsForemIguais_NaoLancarExcecao()
        {
            // Arrange
            var pattern = "^[a-zA-Z]*$";
            var valor = "abc";
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeDiferente(pattern, valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarTamanho_SeStringForMaiorQueMaximo_LancarExcecao()
        {
            // Arrange
            var valor = "123456";
            var maximo = 5;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarTamanho(valor, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarTamanho_SeStringForIgualAoMaximo_NaoLancarExcecao()
        {
            // Arrange
            var valor = "12345";
            var maximo = 5;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarTamanho(valor, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarTamanho_SeStringForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            var valor = "123456";
            var minimo = 7;
            var maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarTamanho(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarTamanho_SeStringForIgualAoMinimo_NaoLancarExcecao()
        {
            // Arrange
            var valor = "123456";
            var minimo = 6;
            var maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarTamanho(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeVazio_SeStringForVazia_LancarExcecao()
        {
            // Arrange
            var valor = "";
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeVazio(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoValidarSeVazio_SeStringNaoForNula_NaoLancarExcecao()
        {
            // Arrange
            string valor = "Teste";
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeVazio(valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public void AoChamarValidarSeNulo_SeObjetoForNulo_LancarExcecao()
        {
            // Arrange
            object objeto = null;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeNulo(objeto, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeNulo_SeObjetoNaoForNulo_NaoLancarExcecao()
        {
            // Arrange
            object objeto = new object();
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeNulo(objeto, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoDouble_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            double valor = 1;
            double minimo = 2;
            double maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoDouble_SeValorForDentroDoPermitido_NaoLancarExcecao()
        {
            // Arrange
            double valor = 5;
            double minimo = 2;
            double maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoFloat_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            float valor = 1;
            float minimo = 2;
            float maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoValidarMinimoMaximo_TipoFloat_SeValorForDentroDoPermitido_NaoLancarExcecao()
        {
            // Arrange
            float valor = 5;
            float minimo = 2;
            float maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoInt_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            int valor = 1;
            int minimo = 2;
            int maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoValidarMinimoMaximo_TipoInt_SeValorForDentroDoPermitido_NaoLancarExcecao()
        {
            // Arrange
            int valor = 5;
            int minimo = 2;
            int maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoLog_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            long valor = 1;
            long minimo = 2;
            long maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoLog_SeValorForDentroDoPermitido_NaoLancarExcecao()
        {
            // Arrange
            long valor = 5;
            long minimo = 2;
            long maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarMinimoMaximo_TipoDecimal_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            decimal valor = 1;
            decimal minimo = 2;
            decimal maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChaamarValidarMinimoMaximo_TipoDecimal_SeValorForDentroDoPermitido_NaoLancarExcecao()
        {
            // Arrange
            decimal valor = 5;
            decimal minimo = 2;
            decimal maximo = 10;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarMinimoMaximo(valor, minimo, maximo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AoChamarValidarSeMenorQue_TipoLong_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            long valor = 1;
            long minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public void AoChamarValidarSeMenorQue_TipoLong_SeValorForMaiorQueMinimo_NaoLancarExcecao()
        {
            // Arrange
            long valor = 5;
            long minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarSeMenorQue_TipoDouble_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            double valor = 1;
            double minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChaamarValidarSeMenorQue_TipoDouble_SeValorForMaiorQueMinimo_NaoLancarExcecao()
        {
            // Arrange
            double valor = 5;
            double minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarSeMenorQue_TipoDecimal_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            decimal valor = 1;
            decimal minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChaamarValidarSeMenorQue_TipoDecimal_SeValorForMaiorQueMinimo_NaoLancarExcecao()
        {
            // Arrange
            decimal valor = 5;
            decimal minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarSeMenorQue_TipoInt_SeValorForMenorQueMinimo_LancarExcecao()
        {
            // Arrange
            int valor = 1;
            int minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void aoChaamarValidarSeMenorQue_TipoInt_SeValorForMaiorQueMinimo_NaoLancarExcecao()
        {
            // Arrange
            int valor = 5;
            int minimo = 2;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeMenorQue(valor, minimo, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarSeFalso_SeValorForFalso_LancarExcecao()
        {
            // Arrange
            var valor = true;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeFalso(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarSeFalso_SeValorForVerdadeiro_NaoLancarExcecao()
        {
            // Arrange
            var valor = false;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeFalso(valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarSeVerdadeiro_SeValorForVerdadeiro_LancarExcecao()
        {
            // Arrange
            var valor = false;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeVerdadeiro(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarSeVerdadeiro_SeValorForFalso_NaoLancarExcecao()
        {
            // Arrange
            var valor = true;
            var mensagem = "Valor nao bate";

            // Act
            try
            {
                Validacoes.ValidarSeVerdadeiro(valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarCPF_SeTamanhoForDiferenteDe11_LancarExcecao()
        {
            // Arrange
            var valor = "123456789";
            var mensagem = "CPF invalido";

            // Act
            try
            {
                Validacoes.ValidarCPF(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarCPF_SeCPFForSequenciaNumerica_LancarExcecao()
        {
            // Arrange
            var valor = "11111111111";
            var mensagem = "CPF invalido";

            // Act
            try
            {
                Validacoes.ValidarCPF(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarCPF_SeCPFForInvalido_LancarExcecao()
        {
            // Arrange
            var valor = "12345678901";
            var mensagem = "CPF invalido";

            // Act
            try
            {
                Validacoes.ValidarCPF(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoValidarCPF_SeCPFForValido_NaoLancarExcecao()
        {
            // Arrange
            var valor = "12345678909";
            var mensagem = "CPF invalido";

            // Act
            try
            {
                Validacoes.ValidarCPF(valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public static void AoChamarValidarEmail_SeEmailTerminarComPonto_LancarExcecao()
        {
            // Arrange
            var valor = "teste.";
            var mensagem = "Email invalido";

            // Act
            try
            {
                Validacoes.ValidarEmail(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }

        }

        [Fact]
        public static void AoChamarValidarEmail_SeEmailNaoForUmMailAddress_LancarExcecao()
        {
            // Arrange
            var valor = "teste@ com";
            var mensagem = "Email invalido";

            // Act
            try
            {
                Validacoes.ValidarEmail(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoChamarValidarEmail_SeEmailForInvalido_LancarExcecao()
        {
            // Arrange
            var valor = "teste";
            var mensagem = "Email invalido";

            // Act
            try
            {
                Validacoes.ValidarEmail(valor, mensagem);
                Assert.True(false);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal(mensagem, ex.Message);
            }
        }

        [Fact]
        public static void AoValidarEmail_SeEmailForValido_NaoLancarExcecao()
        {
            // Arrange
            var valor = "email@email.com.br";
            var mensagem = "Email invalido";

            // Act
            try
            {
                Validacoes.ValidarEmail(valor, mensagem);
                Assert.True(true);
            }
            // Assert
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}