namespace UniDocuments.Text.Domain.Services.Preprocessing.Stemmer;

public interface IStemmer
{
    string Stem(string word);
}