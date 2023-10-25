using Domain.Autenticacao;
using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Autenticacao.UseCases;
using Application.Autenticacao.Commands;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Handlers;
using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Queries;
using Infra.Autenticacao.Repository;
using Infra.Autenticacao;

namespace API.Setup
{
    public static class DependencyInjection
    { 
        public static void RegisterServices(this IServiceCollection services)
        {
            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Domain Notifications 
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Autenticacao
            services.AddTransient<IRequestHandler<AdminAutenticaCommand, LogInUsuarioOutput>, AdminAutenticacaoCommandHandler>();
            services.AddTransient<IRequestHandler<AutenticaClienteCommand, AutenticaClienteOutput>, AutenticaClienteCommandHandler>();
            services.AddTransient<IAutenticacaoRepository, AutenticacaoRepository>();
            services.AddTransient<IRequestHandler<CadastraClienteCommand,AutenticaClienteOutput>,  CadastraClienteCommandHandler>();
            services.AddScoped<IAutenticacaoUseCase, AutenticacaoUseCase>();
            services.AddScoped<IAutenticacaoQuery, AutenticacaoQuery>();
            services.AddScoped<AutenticacaoContext>();
        }
    }
}
