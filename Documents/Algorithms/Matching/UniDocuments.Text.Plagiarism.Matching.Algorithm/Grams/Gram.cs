using System.Text;

namespace UniDocuments.Text.Plagiarism.Matching.Algorithm.Grams;

internal class Gram : IEquatable<Gram>
{
    private readonly IList<GramEntry> _allValues;
    private readonly int _start;
    private readonly int _end;

    public Gram(IList<GramEntry> allValues, int start, int end)
    {
        _allValues = allValues;
        _start = start;
        _end = end;
    }

    public int GetMinPosition()
    {
        return _allValues[_start].Index;
    }

    public int GetMaxPosition()
    {
        var end = _allValues[_end - 1];
        return end.Index + end.Length - 1;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var i = _start; i < _end; i++)
        {
            sb.Append(_allValues[i].ToString());
            sb.Append(' ');
        }

        return sb.ToString();
    }

    public bool Equals(Gram? other)
    {
        if (GetCount() != other?.GetCount())
        {
            return false;
        }
        
        for (var i = _start; i < _end; i++)
        {
            var word = _allValues[i];
            var offset = i - _start;
            var otherWord = other._allValues[other._start + offset];

            if (!word.Equals(otherWord))
            {
                return false;
            }
        }

        return true;
    }

    private int GetCount() => _end - _start + 1;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Gram)obj);
    }

    public override int GetHashCode()
    {
        var hash = 17;
        
        for (var i = _start; i < _end; i++)
        {
            hash = HashCode.Combine(_allValues[i]);
        }

        return hash;
    }
}