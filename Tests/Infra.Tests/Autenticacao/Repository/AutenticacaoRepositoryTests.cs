using Domain.Autenticacao;
using Domain.Autenticacao.Enums;
using Infra.Autenticacao;
using Infra.Autenticacao.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Infra.Tests.Autenticacao.Repository
{
    public class AutenticacaoRepositoryTests : IDisposable
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly DbContextOptions<AutenticacaoContext> _options;

        public AutenticacaoRepositoryTests()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

            _mockConfiguration = new Mock<IConfiguration>();
            _options = new DbContextOptionsBuilder<AutenticacaoContext>()
                .UseInMemoryDatabase(databaseName: "auth")
                .Options;
        }

        #region  AutenticaUsuario
        [Fact]
        public async Task AoAutenticarUsuario_DeveRetornarAcessoUsuario()
        {

            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoUsuario.Add(new AcessoUsuario(
                    Guid.NewGuid(),
                    "fiapUser",
                    "teste",
                    Roles.Gestor
                    ));
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var usuario = await repository.AutenticaUsuario(new AcessoUsuario("fiapUser", "teste"));

                // Assert
                Assert.Equal("fiapUser", usuario.NomeUsuario);
            }
        }

        [Fact]
        public async Task AoAutenticarUsuario_DeveRetornarVazio_Se_UsuarioInvalido()
        {
            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoUsuario.Add(new AcessoUsuario(
                    Guid.NewGuid(),
                    "fiapUser",
                    "teste",
                    Roles.Gestor
                    ));
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var usuario = await repository.AutenticaUsuario(new AcessoUsuario("usuarioInvalido", "teste"));

                // Assert
                Assert.Empty(usuario.NomeUsuario);
            }
        }

        #endregion

        #region  AutenticaCliente
        [Fact]
        public async Task AoAutenticarCliente_DeveRetornarAcessoCliente()
        {

            var cliente = new AcessoCliente(
                                "63852797071",
                                "teste",
                                "teste@teste.com.br",
                                "teste"
                                );
            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoCliente.Add(cliente);
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var usuario = await repository.AutenticaCliente(cliente);

                // Assert
                Assert.Equal(cliente.CPF, usuario.CPF);
            }
        }

        [Fact]
        public async Task AoAutenticarCliente_DeveRetornarVazio_Se_ClienteInvalido()
        {
            var cliente = new AcessoCliente(
                                "47253197836",
                                "teste",
                                "teste@teste.com.br",
                                "teste"
                                );
            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoCliente.Add(cliente);
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var usuario = await repository.AutenticaCliente(new AcessoCliente(
                                "63852797071",
                                "testeinvalido",
                                "testeinvalido@teste.com.br",
                                "testeinvalido"
                                ));

                // Assert
                Assert.Empty(usuario.CPF);
            }
        }
        #endregion

        #region  Cadastra Cliente

        [Fact]
        public async Task AoCadastrarCliente_DeveRetornarVerdadeiro_Se_ClienteJaExistir()
        {
            var cliente = new AcessoCliente(
                                "47253197836",
                                "teste",
                                "teste@teste.com.br",
                                "teste"
                                );
            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoCliente.Add(cliente);
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var resultado = await repository.ClienteJaExiste(cliente);

                // Assert
                Assert.True(resultado);
            }
        }

        [Fact]
        public async Task AoCadastrarCliente_DeveRetornarFalso_Se_ClienteNaoExistir()
        {
            var cliente = new AcessoCliente(
                                "47253197836",
                                "teste",
                                "teste@teste.com.br",
                                "teste"
                                );
            // Povoar o banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                context.AcessoCliente.Add(cliente);
                await context.SaveChangesAsync();
            }

            // Utilizar o contexto populado para o teste
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                var resultado = await repository.ClienteJaExiste(new AcessoCliente(
                                "63852797071",
                                "teste",
                                "testenaoexiste@teste.com.br",
                                "testenaoexiste"
                                ));

                // Assert
                Assert.False(resultado);
            }
        }

        [Fact]
        public async Task AoCadastrarCliente_DeveInserir()
        {
            var cliente = new AcessoCliente(
                    "63852797071",
                    "teste",
                    "teste@teste.com.br",
                    "teste"
                    );

            // Adicionando o produto ao banco de dados em memória
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var repository = new AutenticacaoRepository(context);
                repository.CadastraCliente(cliente);
                context.SaveChanges();
            }

            // Verificando se o produto foi adicionado
            await using (var context = new AutenticacaoContext(_options, _mockConfiguration.Object))
            {
                var clienteAdicionado = await context.AcessoCliente.FirstOrDefaultAsync(p => p.Id == cliente.Id);

                // Assert
                Assert.NotNull(clienteAdicionado);
                Assert.Equal(cliente.CPF, clienteAdicionado!.CPF);
            }
        }

        #endregion

        [Fact]
        public void Dispose_DeveLiberarRecursos()
        {

            var context = new AutenticacaoContext(_options, _mockConfiguration.Object);
            var repository = new AutenticacaoRepository(context);

            // Act
            repository.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => context.Set<AcessoUsuario>().ToList());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
