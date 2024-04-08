namespace UniDocuments.Text.Domain.Services.Common;

public record RawDocument(Guid Id, List<RawParagraph>? Paragraphs)
{
    public bool HasData => Paragraphs is not null;

    public static RawDocument NoData()
    {
        return new RawDocument(Guid.Empty, null);
    }
}