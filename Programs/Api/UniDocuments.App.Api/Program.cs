using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Services.FileStorage.DependencyInjection;
using UniDocuments.Text.Application;
using UniDocuments.Text.Application.Similarity;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Features.Fingerprint;
using UniDocuments.Text.Features.Fingerprint.Services;
using UniDocuments.Text.Features.Text;
using UniDocuments.Text.Features.Text.Services;
using UniDocuments.Text.Features.TextVector;
using UniDocuments.Text.Plagiarism.Cosine.Algorithm;
using UniDocuments.Text.Plagiarism.Matching.Algorithm;
using UniDocuments.Text.Plagiarism.TsSs.Algorithm;
using UniDocuments.Text.Plagiarism.Winnowing.Algorithm;
using UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Hash;
using UniDocuments.Text.Services.Documents;
using UniDocuments.Text.Services.Preprocessing;
using UniDocuments.Text.Services.Preprocessing.Stemming;
using UniDocuments.Text.Services.StreamReading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly));

builder.Services.AddFileStorage(() => builder.Environment.IsDevelopment());
builder.Services.AddStreamContentReader();
builder.Services.AddTextPreprocessor(b =>
{
    b.UseStopWordsService();
    b.UseStemmer<Stemmer>();
});
builder.Services.AddDocumentTextLoader();
builder.Services.AddTextWinnowing(b => b.UseHashAlgorithm<FingerprintHashCrc32C>());
builder.Services.AddFingerprintService();
builder.Services.AddUniDocumentsService();

builder.Services.AddDocumentAlgorithms(algorithmsBuilder =>
{
    algorithmsBuilder
        .UseFeature<UniDocumentFeatureTextFactory>()
        .UseFeature<UniDocumentFeatureFingerprintFactory>()
        .UseSharedFeature<UniDocumentFeatureTextVectorFactory>();

    algorithmsBuilder
        .UseAlgorithm<PlagiarismAlgorithmCosineSimilarity>()
        .UseAlgorithm<PlagiarismAlgorithmMatching>()
        .UseAlgorithm<PlagiarismAlgorithmTsSs>();
    
    algorithmsBuilder
        .UseService<IDocumentSimilarityService, DocumentSimilarityService>();
});

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

var stopWordsLoader = app.Services.GetRequiredService<IStopWordsLoader>();
await stopWordsLoader.LoadStopWordsAsync(CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();