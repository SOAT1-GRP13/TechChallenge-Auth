
using System.Reflection;
using Domain.ValueObjects;
using Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using TechChallengeAuth.Setup;
using Infra.Autenticacao;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

string connectionString = "";
string secret = "";

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAmazonSecretsManager("us-west-2", "auth-secret");
    builder.Services.Configure<Secrets>(builder.Configuration);

    connectionString = builder.Configuration.GetSection("ConnectionString").Value ?? string.Empty;

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}
else
{
    //local
    builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.DatabaseConfiguration));
    connectionString = builder.Configuration.GetSection("ConnectionString").Value ?? string.Empty;

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}

builder.Services.Configure<Secrets>(builder.Configuration);

builder.Services.AddDbContext<AutenticacaoContext>(options =>
         options.UseNpgsql(connectionString));

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddAuthenticationJWT(secret);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UsePathBase(new PathString("/Auth"));
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
using var dbAuthentication = scope.ServiceProvider.GetService<AutenticacaoContext>();

await dbAuthentication!.Database.MigrateAsync();

app.Run();
