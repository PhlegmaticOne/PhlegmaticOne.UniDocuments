namespace UniDocuments.Text.Domain.Services.Preprocessing;

public interface IStemmer
{
    string Stem(string word);
}