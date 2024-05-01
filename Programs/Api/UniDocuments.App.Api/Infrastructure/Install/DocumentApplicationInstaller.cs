using UniDocuments.App.Api.Infrastructure.Services;
using UniDocuments.Text.Application.Comparing;
using UniDocuments.Text.Application.Loading;
using UniDocuments.Text.Application.Matching;
using UniDocuments.Text.Application.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;
using UniDocuments.Text.Root;
using UniDocuments.Text.Services.BaseMetrics;
using UniDocuments.Text.Services.Cache;
using UniDocuments.Text.Services.DocumentMapping;
using UniDocuments.Text.Services.DocumentMapping.Initializers;
using UniDocuments.Text.Services.FileStorage.InMemory;
using UniDocuments.Text.Services.FileStorage.Sql;
using UniDocuments.Text.Services.Fingerprinting;
using UniDocuments.Text.Services.Fingerprinting.Hashing;
using UniDocuments.Text.Services.Fingerprinting.Options;
using UniDocuments.Text.Services.Matching;
using UniDocuments.Text.Services.Matching.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec;
using UniDocuments.Text.Services.Neural.Keras;
using UniDocuments.Text.Services.Neural.Sources;
using UniDocuments.Text.Services.Neural.Vocab;
using UniDocuments.Text.Services.Preprocessing;
using UniDocuments.Text.Services.Preprocessing.Stemming;
using UniDocuments.Text.Services.Preprocessing.StopWords;
using UniDocuments.Text.Services.StreamReading;
using UniDocuments.Text.Services.StreamReading.Options;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class DocumentApplicationInstaller
{
    public static IServiceCollection AddDocumentApplication(this IServiceCollection serviceCollection,
        IConfiguration configuration, string connectionString, bool isDevelopment, ApplicationConfiguration applicationConfiguration)
    {
        serviceCollection.AddDocumentsApplication(appBuilder =>
        {
            appBuilder.UseBaseMetrics<TextSimilarityBaseMetricsProvider>(b =>
            {
                b.UseBaseMetric<TextSimilarityBaseMetricCosine>();
                b.UseBaseMetric<TextSimilarityBaseMetricTsSs>();
                b.UseBaseMetric<TextSimilarityBaseMetricFingerprint>();
            });

            appBuilder.UseDocumentMapper<DocumentMapper>(b =>
            {
                b.UseInitializer<DocumentMappingInitializerNone, DocumentMappingInitializer>(isDevelopment);
            });

            appBuilder.UseDocumentsCache<UniDocumentsCache>();

            appBuilder.UseFileStorage<DocumentsStorageInMemory, DocumentsStorageSql>(applicationConfiguration.UseRealDatabase,
                b => { b.UseSqlConnectionString(connectionString!); });

            appBuilder.UseFingerprint(b =>
            {
                b.UseOptionsProvider<FingerprintOptionsProvider>(configuration);
                b.UseFingerprintAlgorithm<FingerprintWinnowingAlgorithm>();
                b.UseFingerprintComputer<FingerprintComputer>();
                b.UseFingerprintContainer<FingerprintContainer>();
                b.UseFingerprintHash<FingerprintHashCrc32C>();
                b.UseFingerprintSearcher<FingerprintSearcher>();
            });

            appBuilder.UseMatchingService<TextMatchProvider>(b =>
            {
                b.UseOptionsProvider<MatchingOptionsProvider>(configuration);
                b.UseMatchingAlgorithm<TextMatchingAlgorithm>();
            });

            appBuilder.UseNeuralModelProvider<DocumentNeuralModelsProvider>(b =>
            {
                b.UseNeuralModel<DocumentNeuralModelKeras>();
                b.UseNeuralModel<DocumentNeuralModelDoc2Vec>();

                b.UseVocabProvider<DocumentsVocabProvider>();

                b.UseTrainDatasetSource<DocumentTrainDatasetSource>();

                b.BindDoc2VecOptions(configuration, "Doc2VecOptions");
                b.BindKerasOptions(configuration, "KerasOptionsDoc2Vec");
            });

            appBuilder.UseTextPreprocessor<TextPreprocessor>(b =>
            {
                b.UseStemmer<Stemmer>();
                b.UseStopWordsLoader<StopWordsLoaderFile>();
                b.UseStopWordsService<StopWordsService>();
            });

            appBuilder.UseSavePathProvider<SavePathProvider>();

            appBuilder.UseStreamContentReader<StreamContentReaderWordDocument>(b =>
            {
                b.UseOptionsProvider<TextProcessOptionsProvider>(configuration);
            });

            appBuilder.UsePlagiarismSearcher<PlagiarismSearchProvider>();

            appBuilder.UseSimilarityService<TextCompareProvider>();

            appBuilder.UseDocumentLoadingProvider<DocumentLoadingProvider>();
        });

        return serviceCollection;
    }
}