namespace UniDocuments.Text.Domain.Services.Fingerprinting.Options;

public class FingerprintOptions
{
    public double Baseline { get; set; }
    public FingerprintLevelOptions Options { get; set; } = null!;
}