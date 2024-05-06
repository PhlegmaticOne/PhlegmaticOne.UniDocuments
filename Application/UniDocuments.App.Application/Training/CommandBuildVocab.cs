using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Vocab;

namespace UniDocuments.App.Application.Training;

public class CommandBuildVocab : IOperationResultCommand { }

public class CommandBuildVocabHandler : IOperationResultCommandHandler<CommandBuildVocab>
{
    private const string BuildVocabInternalError = "BuildVocab.InternalError";
    
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly IDocumentsTrainDatasetSource _source;
    private readonly ILogger<CommandBuildVocabHandler> _logger;

    public CommandBuildVocabHandler(
        IDocumentsVocabProvider documentsVocabProvider,
        IDocumentsTrainDatasetSource source,
        ILogger<CommandBuildVocabHandler> logger)
    {
        _documentsVocabProvider = documentsVocabProvider;
        _source = source;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandBuildVocab request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _documentsVocabProvider.BuildAsync(_source, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, BuildVocabInternalError);
            return OperationResult.Failed(BuildVocabInternalError, e.Message);
        }
    }
}