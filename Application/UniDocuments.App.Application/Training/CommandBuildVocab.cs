using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural.Vocab;

namespace UniDocuments.App.Application.Training;

public class CommandBuildVocab : IOperationResultCommand { }

public class CommandBuildVocabHandler : IOperationResultCommandHandler<CommandBuildVocab>
{
    private readonly IDocumentsVocabProvider _documentsVocabProvider;

    public CommandBuildVocabHandler(IDocumentsVocabProvider documentsVocabProvider)
    {
        _documentsVocabProvider = documentsVocabProvider;
    }
    
    public async Task<OperationResult> Handle(CommandBuildVocab request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _documentsVocabProvider.BuildAsync(cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed("BuildVocab.InternalError", e.Message);
        }
    }
}