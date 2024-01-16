using Domain.Autenticacao;
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


            // mockRepo.Setup(r => r.com)

            // mockRepo.Setup(r => r.(It.IsAny<AcessoUsuario>())).ReturnsAsync((LeaveType leaveType) => 
            // {
            //     leaveTypes.Add(leaveType);
            //     return leaveType;
            // });

            return mockRepo;
        }
    }
}