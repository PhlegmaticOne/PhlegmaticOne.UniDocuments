using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data;

namespace UniDocuments.Text.Root.Builders;

public class ReportInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public ReportInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseReportDataBuilder<T>() where T : class, IPlagiarismReportDataBuilder
    {
        _serviceCollection.AddScoped<IPlagiarismReportDataBuilder, T>();
    }
    
    public void UseReportCreator<T>() where T : class, IPlagiarismReportCreator
    {
        _serviceCollection.AddScoped<IPlagiarismReportCreator, T>();
    }
}