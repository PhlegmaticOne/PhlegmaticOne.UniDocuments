using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Custom.Core;

namespace UniDocuments.Text.Services.Neural.Custom.Doc2Vec;

public class DocumentNeuralModelCustomDoc2Vec : DocumentNeuralModelCustom<CustomOptionsDoc2Vec>
{
    public DocumentNeuralModelCustomDoc2Vec(
        INeuralOptionsProvider<CustomOptionsDoc2Vec> optionsProvider, 
        IDocumentMapper documentMapper) : 
        base(optionsProvider, documentMapper) { }
}