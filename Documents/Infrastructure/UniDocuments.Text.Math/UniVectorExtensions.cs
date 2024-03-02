using System.Numerics;

namespace UniDocuments.Text.Math;

public static class UniVectorExtensions
{
    public static double Cosine<T>(this UniVector<T> current, UniVector<T> other) where T : INumber<T>
    {
        var dot = current.Dot(other);
        var size = current.Norm();
        var otherSize = other.Norm();
        return double.CreateSaturating(dot) / (size * otherSize);
    }
}