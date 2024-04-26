using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Keras.Core;

namespace UniDocuments.Text.Services.Neural.Keras.Doc2Vec;

public class DocumentNeuralModelKerasDoc2Vec : DocumentNeuralModelKeras<KerasOptionsDoc2Vec>
{
    public DocumentNeuralModelKerasDoc2Vec(
        INeuralOptionsProvider<KerasOptionsDoc2Vec> optionsProvider, 
        IDocumentMapper documentMapper) : 
        base(optionsProvider, documentMapper) { }
}