using System.Numerics;

namespace UniDocuments.Text.Services.BaseMetrics.Infrastructure;

public static class NumberExtensions
{
    public static T Abs<T>(this T a) where T : INumber<T>
    {
        return T.Abs(a);
    }
}