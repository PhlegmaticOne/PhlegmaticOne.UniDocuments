namespace UniDocuments.Text.Domain.Services.Preprocessing.Stemming;

public interface IStemmer
{
    string Stem(string word);
}