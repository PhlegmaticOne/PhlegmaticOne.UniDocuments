using UniDocuments.App.Api.Infrastructure;
using UniDocuments.App.Api.Infrastructure.Extensions;
using UniDocuments.App.Api.Infrastructure.Install;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();
var connectionString = builder.Configuration.GetConnectionString("DbConnection")!;
var applicationConfiguration = builder.Configuration.GetSection<ApplicationConfiguration>()!;
var jwtOptions = applicationConfiguration.CreateJwtOptions();

if (applicationConfiguration.UseAuthentication)
{
    builder.Services.AddAuthentication(jwtOptions);
}

builder.Services.Configure<ApplicationConfiguration>(builder.Configuration.GetSection(nameof(ApplicationConfiguration)));
builder.Services.AddApplication(jwtOptions);
builder.Services.AddDocumentApplication(builder.Configuration, connectionString, isDevelopment, applicationConfiguration);
builder.Services.AddApplicationRequirements(connectionString, applicationConfiguration);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await AppInitializer.InitializeAsync(app, applicationConfiguration, app.Lifetime.ApplicationStopped);

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (applicationConfiguration.UseAuthentication)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

app.Run();