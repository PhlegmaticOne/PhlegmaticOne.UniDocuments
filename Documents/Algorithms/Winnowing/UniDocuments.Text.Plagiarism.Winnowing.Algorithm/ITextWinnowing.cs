using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Fingerprint.Models;

namespace UniDocuments.Text.Plagiarism.Winnowing.Algorithm;

public interface ITextWinnowing
{
    DocumentFingerprint Winnowing(StreamContentReadResult text);
}