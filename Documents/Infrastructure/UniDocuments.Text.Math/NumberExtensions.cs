using System.Numerics;

namespace UniDocuments.Text.Math;

public static class NumberExtensions
{
    public static T Abs<T>(this T a) where T : INumber<T>
    {
        return T.Abs(a);
    }
}