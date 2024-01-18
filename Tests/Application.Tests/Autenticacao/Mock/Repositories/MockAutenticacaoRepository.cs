using Domain.Autenticacao;
using Domain.Base.Data;
using Moq;

namespace Application.Tests.Autenticacao.Mock.Repositories
{
    public static class MockAutenticacaoRepository
    {
        public static Mock<IAutenticacaoRepository> GetAutenticacaoRepository()
        {
            var usuario = new AcessoUsuario("fiapUser", "Teste@123");
            var cliente = new AcessoCliente("63852797071","teste","teste@teste.com.br","teste");

            var mockRepo = new Mock<IAutenticacaoRepository>();

            mockRepo.Setup(r => r.AutenticaUsuario(It.IsAny<AcessoUsuario>())).ReturnsAsync(usuario);

            mockRepo.Setup(r => r.AutenticaCliente(It.IsAny<AcessoCliente>())).ReturnsAsync(cliente);

            mockRepo.Setup(r => r.CadastraCliente(It.IsAny<AcessoCliente>()));
            mockRepo.Setup(r => r.ClienteJaExiste(It.IsAny<AcessoCliente>())).ReturnsAsync(true);

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(u => u.Commit()).ReturnsAsync(true);

            mockRepo.SetupGet(r => r.UnitOfWork).Returns(mockUow.Object);

            return mockRepo;
        }
    }
}