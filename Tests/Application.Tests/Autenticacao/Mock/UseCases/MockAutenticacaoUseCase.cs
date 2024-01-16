using Application.Autenticacao.UseCases;
using Application.Tests.Autenticacao.Mock.Repositories;
using Domain.Autenticacao;
using Domain.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Autenticacao.Mock.UseCases
{
    public static class MockAutenticacaoUseCase
    {
        public static AutenticacaoUseCase GetAutenticacaoUseCase()
        {
            var autenticacaoRepository = MockAutenticacaoRepository.GetAutenticacaoRepository().Object;
            var usuarioLogadoRepository = MockUsuarioLogadoRepository.GetUsuarioLogadoRepository().Object;
            var options = Options.Create(new Secrets()
            {
                ClientSecret = "9%&ujkio7&*(2)@pwqdmndd[lcnslw$01]{&!jd8#}kfgjaprmb2^40+djl=%-hAiO_$u38",
                PreSalt = "qsdc76543$%4&*(kjhgftyumnb#~;",
                PosSalt = "+_)(rtyumlp;~1'123$#@!GYUJN*&"
            });

            return new AutenticacaoUseCase(autenticacaoRepository, usuarioLogadoRepository, options);
        }

        public static Mock<IAutenticacaoUseCase> GetAutencacaoUseCaseMock()
        {
            var mockUseCase = new Mock<IAutenticacaoUseCase>();

            return mockUseCase;
        }
    }
}