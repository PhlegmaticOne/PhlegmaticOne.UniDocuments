using UniDocuments.App.Api.Infrastructure.Services;
using UniDocuments.Text.Application.Comparing;
using UniDocuments.Text.Application.ContentReading;
using UniDocuments.Text.Application.Loading;
using UniDocuments.Text.Application.Matching;
using UniDocuments.Text.Application.PlagiarismSearching;
using UniDocuments.Text.Application.Reports;
using UniDocuments.Text.Domain.Providers.Neural;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;
using UniDocuments.Text.Root;
using UniDocuments.Text.Services.BaseMetrics;
using UniDocuments.Text.Services.Cache;
using UniDocuments.Text.Services.DocumentMapping;
using UniDocuments.Text.Services.DocumentMapping.Initializers;
using UniDocuments.Text.Services.FileStorage.EntityFramework;
using UniDocuments.Text.Services.FileStorage.InMemory;
using UniDocuments.Text.Services.FileStorage.Sql;
using UniDocuments.Text.Services.Fingerprinting;
using UniDocuments.Text.Services.Fingerprinting.Hashing;
using UniDocuments.Text.Services.Fingerprinting.Options;
using UniDocuments.Text.Services.Matching;
using UniDocuments.Text.Services.Matching.Options;
using UniDocuments.Text.Services.Neural;
using UniDocuments.Text.Services.Neural.Doc2Vec;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Keras;
using UniDocuments.Text.Services.Neural.Preprocessors;
using UniDocuments.Text.Services.Neural.Sources;
using UniDocuments.Text.Services.Neural.Vocab;
using UniDocuments.Text.Services.Preprocessing;
using UniDocuments.Text.Services.Preprocessing.Stemming;
using UniDocuments.Text.Services.Preprocessing.StopWords;
using UniDocuments.Text.Services.Reporting;
using UniDocuments.Text.Services.StreamReading;
using UniDocuments.Text.Services.StreamReading.Options;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class DocumentApplicationInstaller
{
    public static IServiceCollection AddDocumentApplication(this IServiceCollection serviceCollection,
        IConfiguration configuration, string connectionString, ApplicationConfiguration applicationConfiguration)
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
                b.UseInitializer<DocumentMappingInitializerNone, DocumentMappingInitializer>(applicationConfiguration.UseRealDatabase);
            });

            appBuilder.UseDocumentsCache<UniDocumentsCache>();

            appBuilder.UseFileStorage<DocumentsStorageInMemory, DocumentsStorageEntityFramework>(applicationConfiguration.UseRealDatabase,
                b =>
                {
                    b.UseSqlConnectionString(connectionString);
                });

            appBuilder.UseFingerprint(b =>
            {
                b.UseOptionsProvider<FingerprintOptionsProvider>(configuration);
                b.UseFingerprintAlgorithm<FingerprintWinnowingAlgorithm>();
                b.UseFingerprintContainer<FingerprintContainer>();
                b.UseFingerprintsProvider<FingerprintsProvider>();
                b.UseFingerprintHash<FingerprintHashCrc32C>();
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
                b.UseVocabProvider<DocumentsVocabProvider>();
                
                b.UseTextPreprocessor<DocumentTextPreprocessor>();
                
                b.UsePlagiarismSearcher<NeuralNetworkPlagiarismSearcher>();

                b.UseTrainDatasetSource<DocumentTrainDatasetSource>();

                b.BindDoc2VecOptions(configuration, nameof(Doc2VecOptions));
                b.BindKerasOptions(configuration, applicationConfiguration.CurrentKerasOptions);
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
                b.UseWordCountApproximator<WordCountApproximatorRegex>();
            });

            appBuilder.UsePlagiarismSearcher<PlagiarismSearchProvider>();

            appBuilder.UseSimilarityService<TextCompareProvider>();

            appBuilder.UseDocumentLoadingProvider<DocumentLoadingProvider>();
            
            appBuilder.UseParagraphGlobalReader<ParagraphGlobalReader>();
            
            appBuilder.UseReportProvider<PlagiarismReportProviderWord>(b =>
            {
                b.UseReportDataBuilder<PlagiarismReportDataBuilder>();
            });
        });

        return serviceCollection;
    }
}