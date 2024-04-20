using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.JwtTokensGeneration.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PasswordHasher.Implementation;
using UniDocuments.App.Api.Services;
using UniDocuments.App.Api.Services.Initializer;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Services.Jwt;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;
using UniDocuments.Text.Providers.PlagiarismSearching;
using UniDocuments.Text.Providers.Similarity;
using UniDocuments.Text.Root;
using UniDocuments.Text.Services.BaseMetrics;
using UniDocuments.Text.Services.DocumentMapping;
using UniDocuments.Text.Services.Documents;
using UniDocuments.Text.Services.FileStorage.InMemory;
using UniDocuments.Text.Services.FileStorage.Sql;
using UniDocuments.Text.Services.Fingerprinting;
using UniDocuments.Text.Services.Matching;
using UniDocuments.Text.Services.Matching.Options;
using UniDocuments.Text.Services.Neural.Models;
using UniDocuments.Text.Services.Neural.Services;
using UniDocuments.Text.Services.Preprocessing;
using UniDocuments.Text.Services.Preprocessing.Stemming;
using UniDocuments.Text.Services.Preprocessing.StopWords;
using UniDocuments.Text.Services.StreamReading;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var isDevelopment = builder.Environment.IsDevelopment();
var jwtSecrets = configuration.GetSection("JwtSecrets");
var jwtOptions = new SymmetricKeyJwtOptions(jwtSecrets["Issuer"]!,
    jwtSecrets["Audience"]!,
    int.Parse(jwtSecrets["ExpirationDurationInMinutes"]!),
    jwtSecrets["SecretKey"]!);

if (!isDevelopment)
{
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = jwtOptions.GetSecretKey(),
            ClockSkew = TimeSpan.Zero
        };
    });
}

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly);
});

builder.Services.AddDocumentsApplication(appBuilder =>
{
    appBuilder.UseBaseMetrics<TextSimilarityBaseMetricsProvider>(b =>
    {
        b.UseBaseMetric<TextSimilarityBaseMetricCosine>();
        b.UseBaseMetric<TextSimilarityBaseMetricTsSs>();
        b.UseBaseMetric<TextSimilarityBaseMetricFingerprint>();
    });
    
    appBuilder.UseDocumentMapper<DocumentMapperInMemory, DocumentMapperSql>(isDevelopment);
    
    appBuilder.UseDocumentsService<UniDocumentsService>(b =>
    {
        b.UseDocumentsCache<UniDocumentsCache>();
    });
    
    appBuilder.UseFileStorage<FileStorageInMemory, FileStorageSql>(isDevelopment, b =>
    {
        b.UseSqlConnectionProvider<SqlConnectionProvider>();
    });
    
    appBuilder.UseFingerprint(b =>
    {
        b.UseOptionsProvider<FingerprintOptionsProvider>(builder.Configuration);
        b.UseFingerprintAlgorithm<FingerprintWinnowingAlgorithm>();
        b.UseFingerprintComputer<FingerprintComputer>();
        b.UseFingerprintContainer<FingerprintsContainer>();
        b.UseFingerprintHash<FingerprintHashCrc32C>();
        b.UseFingerprintSearcher<FingerprintSearcher>();
    });
    
    appBuilder.UseMatchingService<TextMatchingService>(b =>
    {
        b.UseOptionsProvider<MatchingOptionsProvider>(builder.Configuration);
        b.UseMatchingAlgorithm<TextMatchingAlgorithm>();
    });

    appBuilder.UseNeuralModel<DocumentNeuralModel>(b =>
    {
        b.UseDataHandler<DocumentsNeuralDataHandler>();
        b.UseDataSource<DocumentNeuralSourceInMemory, DocumentNeuralSourceSql>(isDevelopment);
        b.UseOptionsProvider<DocumentNeuralOptionsProvider>(builder.Configuration);
    });
    
    appBuilder.UseTextPreprocessor<TextPreprocessor>(b =>
    {
        b.UseStemmer<Stemmer>();
        b.UseStopWordsLoader<StopWordsLoaderFile>();
        b.UseStopWordsService<StopWordsService>();
    });

    appBuilder.UseSavePathProvider<SavePathProvider>();
    
    appBuilder.UseStreamContentReader<StreamContentReaderWordDocument>();
    
    appBuilder.UsePlagiarismSearcher<PlagiarismSearcher>();
    
    appBuilder.UseSimilarityService<CompareTextsService>();
});

builder.Services.AddSingleton<IPasswordHasher, SecurePasswordHasher>();
builder.Services.AddSingleton<IJwtTokenGenerationService, JwtTokenGenerationService>();
builder.Services.AddJwtTokenGeneration(jwtOptions);
builder.Services.AddSingleton<IAppInitializer, AppInitializer>();

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

var appInitializer = app.Services.GetRequiredService<IAppInitializer>();
await appInitializer.InitializeAsync(app.Lifetime.ApplicationStopped);

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!isDevelopment)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

app.Run();