namespace PhlegmaticOne.UniDocuments.Documents.Core.Algorithms;

public interface IPlagiarismAlgorithm
{
    IPlagiarismResult Perform(UniDocument comparing, UniDocument original);
}