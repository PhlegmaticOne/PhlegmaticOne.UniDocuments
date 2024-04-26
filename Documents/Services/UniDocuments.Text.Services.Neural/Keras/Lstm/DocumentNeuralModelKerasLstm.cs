using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Keras.Core;

namespace UniDocuments.Text.Services.Neural.Keras.Lstm;

public class DocumentNeuralModelKerasLstm : DocumentNeuralModelKeras<KerasOptionsLstm>
{
    public DocumentNeuralModelKerasLstm(
        INeuralOptionsProvider<KerasOptionsLstm> optionsProvider, 
        IDocumentMapper documentMapper) : 
        base(optionsProvider, documentMapper) { }
}