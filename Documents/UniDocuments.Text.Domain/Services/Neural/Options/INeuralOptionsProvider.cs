namespace UniDocuments.Text.Domain.Services.Neural.Options;

public interface INeuralOptionsProvider<out T> where T : INeuralOptions
{
    T GetOptions();
}