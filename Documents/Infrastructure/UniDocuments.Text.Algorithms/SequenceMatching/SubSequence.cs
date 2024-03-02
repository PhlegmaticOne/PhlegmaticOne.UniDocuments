namespace UniDocuments.Text.Algorithms.SequenceMatching;

public struct SubSequence : IEquatable<SubSequence>, IFormattable
{
    public int LeftIndex;
    public int RightIndex;
    public int LeftLength;
    public int RightLength;
    public int LeftEndIndex => LeftIndex + LeftLength - 1;
    public int RightEndIndex => RightIndex + RightLength - 1;

    public int Size => Math.Min(LeftLength, RightLength);

    public SubSequence(int leftIndex, int rightIndex, int commonLength) :
        this(leftIndex, rightIndex, commonLength, commonLength) { }
    
    public SubSequence(int leftIndex, int rightIndex, int leftLength, int rightLength)
    {
        LeftIndex = leftIndex;
        RightIndex = rightIndex;
        LeftLength = leftLength;
        RightLength = rightLength;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 23 + LeftIndex;
        hash = hash * 23 + RightIndex;
        hash = hash * 23 + LeftLength;
        hash = hash * 23 + RightLength;
        return hash;
    }

    public override bool Equals(object? obj)
    {
        return obj is SubSequence sequence && Equals(sequence);
    }

    public bool Equals(SubSequence other)
    {
        return other.LeftLength == LeftLength && other.RightLength == RightLength &&
               other.LeftIndex == LeftIndex && other.RightIndex == RightIndex;
    }

    public override string ToString() => ToString("S", null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "S";
        
        switch (format)
        {
            case "S":
            {
                if (LeftIndex < 0 && RightIndex < 0)
                {
                    return "(<>)";
                }

                if (LeftIndex < 0)
                {
                    return RightLength < 1
                        ? string.Format(formatProvider, "(> {0})", RightIndex)
                        : string.Format(formatProvider, "(> {0} -> {1})", RightIndex, RightEndIndex);
                }

                if (RightIndex < 0)
                {
                    return LeftLength < 1
                        ? string.Format(formatProvider, "(< {0})", LeftIndex)
                        : string.Format(formatProvider, "(< {0} -> {1})", LeftIndex, LeftEndIndex);
                }

                if (LeftLength < 1 && RightLength < 1)
                {
                    return string.Format(formatProvider, "({0}, {1})", LeftIndex, RightIndex);
                }

                if (LeftLength < 1)
                {
                    return string.Format(formatProvider, "({0}, {1} -> {2})", LeftIndex, RightIndex,
                        RightIndex + RightLength - 1);
                }

                if (RightLength < 1)
                {
                    return string.Format(formatProvider, "({0} -> {1}, {2})", LeftIndex, LeftEndIndex, RightIndex);
                }

                return string.Format(formatProvider, "({0} -> {1}, {2} -> {3})",
                    LeftIndex, LeftEndIndex, RightIndex, RightEndIndex);
            }
            case "T":
            {
                return string.Format(formatProvider, "{0}, {1}, {2}, {3}", LeftIndex, RightIndex, LeftLength,
                    RightLength);
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(format));
            }
        }
    }

    public static bool operator ==(SubSequence left, SubSequence right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SubSequence left, SubSequence right)
    {
        return !(left == right);
    }
}