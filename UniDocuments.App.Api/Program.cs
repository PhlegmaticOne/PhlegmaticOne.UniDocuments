using UniDocuments.App.Api.Infrastructure;

var app = await WebApplication
    .CreateBuilder(args)
    .InstallRequirements()
    .Build()
    .InitializeAsync();

app.Start();