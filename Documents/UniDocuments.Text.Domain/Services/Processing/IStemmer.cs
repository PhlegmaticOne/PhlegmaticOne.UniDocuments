namespace UniDocuments.Text.Domain.Services.Processing;

public interface IStemmer
{
    string Stem(string word);
}