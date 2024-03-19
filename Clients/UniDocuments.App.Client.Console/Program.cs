using RestSharp;
using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Services;
using UniDocuments.Text.Features.Text.Contracts;

const string path = @"C:\Users\lolol\Downloads\test.docx";
var client = new RestClient("http://localhost:5109");
    
var request = new RestRequest("/UniDocuments/UploadFile")
    .AddFile("formFile", path, "multipart/form-data");
    
var response = await client.PostAsync(request);
Console.WriteLine("response :" + response.Content);

// var stopWordsService = new StopWordsService(new StopWordsLoaderFile());
// var service = new UniDocumentService(cache);
//
// var featureFactories = new List<IUniDocumentFeatureFactory>
// {
//     new UniDocumentFeatureTextFactory(new TextLoader()),
// };
//
// var sharedFeatureFactories = new List<IUniDocumentSharedFeatureFactory>
// {
//     new UniDocumentFeatureTextVectorFactory(preprocessor)
// };
//
// var algorithms = new List<IPlagiarismAlgorithm>
// {
//     new PlagiarismAlgorithmCosineSimilarity(),
//     new PlagiarismAlgorithmTsSs(),
//     new PlagiarismAlgorithmMatching(stopWordsService)
// };
//
// await stopWordsService.InitializeAsync(CancellationToken.None);
//
// var provider = new UniDocumentFeatureProvider(featureFactories, sharedFeatureFactories);
//
// var tasks = new Tasks(service, algorithms, provider);
//
// var result = await tasks.CompareDocuments(Consts.OriginalGuid, Consts.PlagiatedGuid, new[]
// {
//     "CosineSimilarity", "TsSs"
// });
//
// foreach (var plagiarismResult in result.GetResults())
// {
//     Console.WriteLine(plagiarismResult);
// }

public class Consts
{
    public static Guid OriginalGuid = new("7c8345ec-9686-47ee-a353-94222086df6a");
    public static Guid PlagiatedGuid = new("b6356a6e-7d98-4d5b-9811-c7bea00801ee");
}

class TextLoader : IDocumentTextLoader
{
    private readonly Dictionary<Guid, string> _paths = new()
    {
        {
            Consts.OriginalGuid,
            "C:\\Users\\lolol\\Downloads\\Plagiarism-Detection-master\\Plagiarism-Detection-master\\data\\orig_taskb.txt"
        },
        {
            Consts.PlagiatedGuid,
            "C:\\Users\\lolol\\Downloads\\Plagiarism-Detection-master\\Plagiarism-Detection-master\\data\\g0pA_taskb.txt"
        },
    };
    
    public Task<string> LoadTextAsync(Guid documentId)
    {
        var path = _paths[documentId];
        return File.ReadAllTextAsync(path);
    }
}

class Cache : IUniDocumentsCache
{
    private readonly Dictionary<Guid, UniDocument> _documents = new();
    public Task CacheDocumentAsync(UniDocument document)
    {
        _documents[document.Id] = document;
        return Task.CompletedTask;
    }

    public Task<UniDocument?> GetDocumentAsync(Guid documentId)
    {
        var document = _documents.GetValueOrDefault(documentId);
        return Task.FromResult(document);
    }
}
