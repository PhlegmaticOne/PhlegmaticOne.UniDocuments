using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.Text.Domain.Services.Reading;

namespace UniDocuments.App.Application.Loading.Commands;

public class CommandUploadDocument : IdentityOperationResultCommand
{
    public Stream DocumentStream { get; }

    public CommandUploadDocument(Guid profileId, Stream documentStream) : base(profileId)
    {
        DocumentStream = documentStream;
    }
}

public class CommandUploadDocumentHandler : IOperationResultCommandHandler<CommandUploadDocument>
{
    private readonly IFileStorage _fileStorage;
    private readonly IStreamContentReader _streamContentReader;

    public CommandUploadDocumentHandler(IFileStorage fileStorage, IStreamContentReader streamContentReader)
    {
        _fileStorage = fileStorage;
        _streamContentReader = streamContentReader;
    }
    
    public async Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        //var saveRequest = new FileSaveRequest(string.Empty, request.DocumentStream);
        //var fileId = await _fileStorage.SaveAsync(saveRequest, cancellationToken);
        var content = await _streamContentReader.ReadAsync(request.DocumentStream, cancellationToken);
        return OperationResult.Successful(true);
    }
}