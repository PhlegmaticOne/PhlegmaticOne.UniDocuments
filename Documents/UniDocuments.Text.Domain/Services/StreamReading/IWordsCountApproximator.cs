namespace UniDocuments.Text.Domain.Services.StreamReading;

public interface IWordsCountApproximator
{
    int ApproximateWordsCount(string text);
}