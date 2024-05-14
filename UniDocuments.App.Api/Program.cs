using UniDocuments.App.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var applicationConfiguration = AppInstaller.Install(builder);
var app = builder.Build();
await AppInitializer.InitializeAsync(app, applicationConfiguration);
AppRunner.Run(app, applicationConfiguration);