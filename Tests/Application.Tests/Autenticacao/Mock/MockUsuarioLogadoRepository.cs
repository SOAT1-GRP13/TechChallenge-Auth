using Domain.Autenticacao;
using Moq;

namespace Application.Tests.Autenticacao.Mock
{
    public static class MockUsuarioLogadoRepository
    {
        public static Mock<IUsuarioLogadoRepository> GetUsuarioLogadoRepository()
        {
            var mockRepo = new Mock<IUsuarioLogadoRepository>();

            mockRepo.Setup(r => r.AddUsuarioLogado("testetoken")).ReturnsAsync(true);

            return mockRepo;
        }
    }
}