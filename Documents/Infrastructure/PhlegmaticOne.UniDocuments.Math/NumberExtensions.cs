using System.Numerics;

namespace PhlegmaticOne.UniDocuments.Math;

public static class NumberExtensions
{
    public static T Abs<T>(this T a) where T : INumber<T>
    {
        return T.Abs(a);
    }
}