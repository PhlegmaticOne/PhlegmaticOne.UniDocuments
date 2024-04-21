using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Custom.Core;

namespace UniDocuments.Text.Services.Neural.Custom.Lstm;

public class DocumentNeuralModelCustomLstm : DocumentNeuralModelCustom<CustomOptionsLstm>
{
    public DocumentNeuralModelCustomLstm(
        INeuralOptionsProvider<CustomOptionsLstm> optionsProvider, 
        IDocumentMapper documentMapper) : 
        base(optionsProvider, documentMapper) { }
}