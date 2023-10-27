using Amazon.DynamoDBv2;
using System.Reflection;
using Infra.Autenticacao;
using Domain.ValueObjects;
using TechChallengeAuth.Setup;
using Amazon.DynamoDBv2.DataModel;
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
        var PostgressConnectionString = Configuration.GetSection("DatabaseSettings:PostgresString").Value;
        var DynamoDbConnectionString = Configuration.GetSection("DatabaseSettings:DynamoDBString").Value;

        string secret = GetSecret();

        services.AddDbContext<AutenticacaoContext>(options =>
                options.UseNpgsql(PostgressConnectionString));

        //Add DynamoDB configuration
        var awsOptions = Configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();

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
            c.SwaggerEndpoint("v1/swagger.json", "Auth API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", context => {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
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