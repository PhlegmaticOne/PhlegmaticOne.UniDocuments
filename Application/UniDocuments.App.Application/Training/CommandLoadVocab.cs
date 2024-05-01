using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Training;

public class CommandLoadVocab : IOperationResultCommand { }

public class CommandLoadVocabHandler : IOperationResultCommandHandler<CommandLoadVocab>
{
    private readonly IDocumentsVocabProvider _documentsVocabProvider;

    public CommandLoadVocabHandler(IDocumentsVocabProvider documentsVocabProvider)
    {
        _documentsVocabProvider = documentsVocabProvider;
    }
    
    public async Task<OperationResult> Handle(CommandLoadVocab request, CancellationToken cancellationToken)
    {
        try
        {
            await _documentsVocabProvider.LoadAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            return OperationResult.Failed("LoadVocab.InternalError", e.Message);
        }
    }
}