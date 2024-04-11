using Microsoft.EntityFrameworkCore;
using UniDocuments.App.Api.Controllers;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Features.Fingerprint.Services;
using UniDocuments.Text.Features.Text.Services;
using UniDocuments.Text.Plagiarism.Cosine.Algorithm;
using UniDocuments.Text.Plagiarism.Matching.Algorithm;
using UniDocuments.Text.Plagiarism.Matching.Services;
using UniDocuments.Text.Plagiarism.TsSs.Algorithm;
using UniDocuments.Text.Providers.PlagiarismSearching;
using UniDocuments.Text.Providers.Similarity;
using UniDocuments.Text.Root;
using UniDocuments.Text.Services.DocumentNameMapping;
using UniDocuments.Text.Services.Documents;
using UniDocuments.Text.Services.FileStorage.InMemory;
using UniDocuments.Text.Services.FileStorage.Sql;
using UniDocuments.Text.Services.Neural.Services;
using UniDocuments.Text.Services.Preprocessing;
using UniDocuments.Text.Services.Preprocessing.Stemming;
using UniDocuments.Text.Services.Preprocessing.StopWords;
using UniDocuments.Text.Services.StreamReading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly));

builder.Services.AddDocumentsApplication(appBuilder =>
{
    var isDevelopment = builder.Environment.IsDevelopment();
    
    appBuilder.UseAlgorithm<PlagiarismAlgorithmCosineSimilarity>();
    
    appBuilder.UseAlgorithm<PlagiarismAlgorithmTsSs>();
    
    appBuilder.UseMatchingAlgorithm(b =>
    {
        b.UseOptionsProvider<MatchingOptionsProvider>(builder.Configuration);
    });
    
    appBuilder.UseTextVectorFeature();
    
    appBuilder.UseTextFeature(b =>
    {
        b.UseTextLoader<DocumentTextLoader>();
    });

    appBuilder.UseFingerprintFeature(b =>
    {
        b.UseFingerprintAlgorithm<FingerprintWinnowingAlgorithm>();
        b.UseFingerprintContainer<FingerprintsContainer>();
        b.UseFingerprintComputer<FingerprintComputer>();
        b.UseFingerprintHash<FingerprintHashCrc32C>();
        b.UseFingerprintSearcher<FingerprintSearcher>();
        b.UseOptionsProvider<FingerprintOptionsProvider>(builder.Configuration);
    });

    appBuilder.UseDocumentsService<UniDocumentsService>(b =>
    {
        b.UseDocumentsCache<UniDocumentsCache>();
    });
    
    appBuilder.UseFileStorage<FileStorageInMemory, FileStorageSql>(isDevelopment, b =>
    {
        b.UseSqlConnectionProvider<SqlConnectionProvider>();
    });
    
    appBuilder.UseTextPreprocessor<TextPreprocessor>(b =>
    {
        b.UseStemmer<Stemmer>();
        b.UseStopWordsLoader<StopWordsLoaderFile>();
        b.UseStopWordsService<StopWordsService>();
    });
    
    appBuilder.UseNeuralModel<DocumentNeuralModel>(b =>
    {
        b.UseDataHandler<DocumentsNeuralDataHandler>();
        b.UseDataSource<DocumentNeuralSourceInMemory, DocumentNeuralSourceSql>(isDevelopment);
        b.UseOptionsProvider<DocumentNeuralOptionsProvider>(builder.Configuration);
    });
    
    appBuilder.UseDocumentNameMapper<DocumentToNameMapperInMemory, DocumentToNameMapperSql>(isDevelopment);
    appBuilder.UseSavePathProvider<SavePathProvider>();
    appBuilder.UseStreamContentReader<StreamContentReaderWordDocument>();
    
    appBuilder.UsePlagiarismFinder<PlagiarismFinder>();
    appBuilder.UseSimilarityFinder<DocumentsSimilarityFinder>();
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