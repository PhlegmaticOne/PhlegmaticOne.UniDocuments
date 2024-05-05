using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Vocab;

namespace UniDocuments.App.Application.Training;

public class CommandBuildVocab : IOperationResultCommand { }

public class CommandBuildVocabHandler : IOperationResultCommandHandler<CommandBuildVocab>
{
    private readonly IDocumentsVocabProvider _documentsVocabProvider;
    private readonly IDocumentsTrainDatasetSource _source;

    public CommandBuildVocabHandler(IDocumentsVocabProvider documentsVocabProvider, IDocumentsTrainDatasetSource source)
    {
        _documentsVocabProvider = documentsVocabProvider;
        _source = source;
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
            return OperationResult.Failed("BuildVocab.InternalError", e.Message);
        }
    }
}