using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Services.FileStorage.DependencyInjection;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Services.StreamReading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly);
});

builder.Services.AddFileStorage(() => builder.Environment.IsDevelopment());

builder.Services.AddSingleton<IStreamContentReader, StreamContentReaderWordDocument>();

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    if (builder.Environment.IsDevelopment())
    {
        x.UseInMemoryDatabase("MEMORY");
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DbConnection");
        x.UseSqlServer(connectionString!);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();