using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Api.Infrastructure.Configurations;

public class ApplicationSettings
{
    public Student Admin { get; set; } = null!;
    public Teacher Teacher { get; set; } = null!;
    public StudyActivity DefaultActivity { get; set; } = null!;
}