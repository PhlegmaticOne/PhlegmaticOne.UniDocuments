using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services.Statistics;
using UniDocuments.App.Shared.Shared;

namespace UniDocuments.App.Application.App.Statistics;

public class QueryGetStatistics : IOperationResultQuery<StatisticsData> { }

public class QueryGetStatisticsHandler : IOperationResultQueryHandler<QueryGetStatistics, StatisticsData>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPlatformActiveTimeProvider _activeTimeProvider;

    public QueryGetStatisticsHandler(
        ApplicationDbContext dbContext,
        IPlatformActiveTimeProvider activeTimeProvider)
    {
        _dbContext = dbContext;
        _activeTimeProvider = activeTimeProvider;
    }
    
    public async Task<OperationResult<StatisticsData>> Handle(
        QueryGetStatistics request, CancellationToken cancellationToken)
    {
        return OperationResult.Successful(new StatisticsData
        {
            ActivitiesCount = await _dbContext.Set<StudyActivity>().CountAsync(cancellationToken),
            DocumentsLoaded = await _dbContext.Set<StudyDocument>().CountAsync(cancellationToken),
            StudentsCount = await _dbContext.Set<Student>().CountAsync(cancellationToken),
            TeachersCount = await _dbContext.Set<Teacher>().CountAsync(cancellationToken),
            PlatformActiveTime = _activeTimeProvider.GetActiveTime()
        });
    }
}