using System.Numerics;

namespace UniDocuments.Text.Math;

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

    public double EuclideanDistance(UniVector<T> other)
    {
        if (other._vector.Length != _vector.Length)
        {
            return 0;
        }
        
        var result = T.Zero;

        for (var i = 0; i < _vector.Length; i++)
        {
            var value = _vector[i] - other._vector[i];
            result += value * value;
        }

        var asDouble = double.CreateSaturating(result);
        return System.Math.Sqrt(asDouble);
    }

    public double Norm()
    {
        var result = T.Zero;

        for (var i = 0; i < _vector.Length; i++)
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

        for (var i = 0; i < other._vector.Length; i++)
        {
            result += _vector[i] * other._vector[i];
        }

        return result;
    }
}
