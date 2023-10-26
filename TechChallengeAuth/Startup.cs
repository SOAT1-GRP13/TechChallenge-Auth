using System.Reflection;
using Infra.Autenticacao;
using Domain.ValueObjects;
using TechChallengeAuth.Setup;
using Microsoft.EntityFrameworkCore;

namespace TechChallengeAuth;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<DatabaseSettings>(Configuration.GetSection(DatabaseSettings.DatabaseConfiguration));
        var connectionString = Configuration.GetSection("DatabaseSettings:ConnectionString").Value;

        string secret = GetSecret();

        services.AddDbContext<AutenticacaoContext>(options =>
                options.UseNpgsql(connectionString));

        services.Configure<ConfiguracaoToken>(Configuration.GetSection(ConfiguracaoToken.Configuration));
        services.AddAuthenticationJWT(secret);

        services.AddControllers();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.RegisterServices();
        services.AddSwaggerGenConfig();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    #region Metodos privados
    private string GetSecret()
    {
        var secret = Configuration.GetSection("ConfiguracaoToken:ClientSecret").Value;

        if (string.IsNullOrEmpty(secret))
            throw new Exception("Secret não configurado");

        return secret.ToString();
    }
    #endregion
}