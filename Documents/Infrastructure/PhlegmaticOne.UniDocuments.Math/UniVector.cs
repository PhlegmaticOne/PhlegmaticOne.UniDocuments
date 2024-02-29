using System.Numerics;

namespace PhlegmaticOne.UniDocuments.Math;

public class UniVector<T> where T : INumber<T>
{
    private readonly T[] _vector;

    public UniVector(T[] vector)
    {
        _vector = vector;
    }

    public static UniVector<T> FromEnumerating<TValue>(ICollection<TValue> values, Func<TValue, T> selector)
    {
        var i = 0;
        var size = values.Count;
        var vector = new T[size];

        foreach(var value in values)
        {
            vector[i] = selector(value);
            ++i;
        }

        return new UniVector<T>(vector);
    }

    public double Norm()
    {
        var result = T.Zero;

        for (int i = 0; i < _vector.Length; i++)
        {
            result += _vector[i] * _vector[i];
        }

        var asDouble = double.CreateSaturating(result);
        return System.Math.Sqrt(asDouble);
    }

    public T Dot(UniVector<T> other)
    {
        if (_vector.Length != other._vector.Length)
        {
            return T.Zero;
        }

        var result = T.Zero;

        for (int i = 0; i < other._vector.Length; i++)
        {
            result += _vector[i] * other._vector[i];
        }

        return result;
    }
}
