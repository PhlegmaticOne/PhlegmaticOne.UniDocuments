﻿using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Providers.Reports.Builder;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.Text.Root.Builders;

public class ReportInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public ReportInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseReportDataBuilder<T>() where T : class, IReportDataBuilder
    {
        _serviceCollection.AddScoped<IReportDataBuilder, T>();
    }
    
    public void UseReportDataLoader<T>() where T : class, IReportDataLoader
    {
        _serviceCollection.AddScoped<IReportDataLoader, T>();
    }
    
    public void UseReportCreator<T>() where T : class, IReportCreator
    {
        _serviceCollection.AddScoped<IReportCreator, T>();
    }
}