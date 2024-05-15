using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Documents;

namespace UniDocuments.App.Application.Documents.Data;

public class QueryGetGlobalData : IOperationResultQuery<DocumentsGlobalData> { }

public class QueryGetGlobalDataHandler : IOperationResultQueryHandler<QueryGetGlobalData, DocumentsGlobalData>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetGlobalDataHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<DocumentsGlobalData>> Handle(QueryGetGlobalData request, CancellationToken cancellationToken)
    {
        var set = _dbContext.Set<StudyDocument>();
        var count = await set.CountAsync(cancellationToken);
        var average = count == 0 ? 0 : await set.AverageAsync(x => x.ValuableParagraphsCount, cancellationToken);

        return OperationResult.Successful(new DocumentsGlobalData
        {
            DocumentsCount = count,
            AverageParagraphsCount = average
        });
    }
}