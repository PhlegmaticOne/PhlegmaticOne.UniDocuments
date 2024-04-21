namespace UniDocuments.Text.Services.BaseMetrics.Infrastructure;

public readonly partial struct UniAngle : IEquatable<UniAngle>
{
    // ReSharper disable once InconsistentNaming
    public const double PI = System.Math.PI;
    
    public readonly double ValueRadians;

    private UniAngle(in double valueRadians) => ValueRadians = valueRadians;

    public static UniAngle FromDegrees(in double degrees) => new(ToRadians(degrees));
    public static UniAngle FromRadians(in double radians) => new(radians);
    public static UniAngle operator +(in UniAngle a, in UniAngle b) => new(a.ValueRadians + b.ValueRadians);
    public static UniAngle operator -(in UniAngle a, in UniAngle b) => new(a.ValueRadians - b.ValueRadians);
    public double SegmentSquare(in double segmentRadius) => PI * segmentRadius * segmentRadius * ToDegrees() / 360;
    public double TriangleSquare(in double sideA, in double sideB) => sideA * sideB * ToSinus() / 2;
    public double ToSinus() => System.Math.Sin(ValueRadians);
    public double ToCosine() => System.Math.Cos(ValueRadians);
    public double ToDegrees() => ToDegrees(ValueRadians);
    private static double ToDegrees(double radians) => radians * 180 / PI;
    private static double ToRadians(double degrees) => degrees * PI / 180;
    public bool Equals(UniAngle other) => ValueRadians.Equals(other.ValueRadians);
    public override bool Equals(object? obj) => obj is UniAngle other && Equals(other);
    public override int GetHashCode() => ValueRadians.GetHashCode();
    public static bool operator ==(UniAngle left, UniAngle right) => left.Equals(right);
    public static bool operator !=(UniAngle left, UniAngle right) => !(left == right);
}

public readonly partial struct UniAngle
{
    public static UniAngle ArcCos(double angle)
    {
        var acos = System.Math.Acos(angle);
        return FromRadians(acos);
    }
    
    public static UniAngle ArcSin(double angle)
    {
        var asin = System.Math.Asin(angle);
        return FromRadians(asin);
    }
}