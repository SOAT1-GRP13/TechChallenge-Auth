using System.Reflection;
using Infra.Autenticacao;
using Domain.ValueObjects;
using TechChallengeAuth.Setup;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

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
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSwaggerGenConfig();

        services.RegisterServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger(swagger =>
        {
            swagger.RouteTemplate = "swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "API V1");
            c.RoutePrefix = "swagger";
        });

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
    }

    #region Metodos privados
    private string GetSecret()
    {
        var secret = Configuration.GetSection("ConfiguracaoToken:ClientSecret").Value;

        if (string.IsNullOrEmpty(secret))
            throw new Exception("Secret n�o configurado");

        return secret.ToString();
    }
    #endregion
}