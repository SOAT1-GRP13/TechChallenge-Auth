using MediatR;
using Infra.Autenticacao;
using Domain.Autenticacao;
using Infra.Autenticacao.Repository;
using Application.Autenticacao.Queries;
using Application.Autenticacao.Handlers;
using Application.Autenticacao.UseCases;
using Application.Autenticacao.Commands;
using Domain.Base.Communication.Mediator;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Boundaries.Cliente;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace TechChallengeAuth.Setup
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
            services.AddTransient<IRequestHandler<AutenticaClientePorNomeCommand, AutenticaClienteOutput>, AutenticaClientePorNomeCommandHandler>();
            services.AddTransient<IAutenticacaoRepository, AutenticacaoRepository>();
            services.AddTransient<IUsuarioLogadoRepository, UsuarioLogadoRepository>();
            services.AddTransient<IRequestHandler<CadastraClienteCommand,AutenticaClienteOutput>,  CadastraClienteCommandHandler>();
            services.AddTransient<IRequestHandler<AnonimizarClienteCommand, bool>, AnonimizarClienteCommandHandler>();
            services.AddScoped<IAutenticacaoUseCase, AutenticacaoUseCase>();
            services.AddScoped<IAutenticacaoQuery, AutenticacaoQuery>();
            services.AddScoped<AutenticacaoContext>();
        }
    }
}
