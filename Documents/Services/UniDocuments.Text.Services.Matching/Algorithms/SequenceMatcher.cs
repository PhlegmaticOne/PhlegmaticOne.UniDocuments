namespace UniDocuments.Text.Services.Matching.Algorithms;

public static class SequenceMatcher
{
    #region Structs
    private readonly struct NullablePair
    {
        public readonly int? A;
        public readonly int? B;

        public NullablePair(int? a, int? b)
        {
            A = a;
            B = b;
        }

        public NullablePair SetA(int? a) => new(a, B);
        public NullablePair SetB(int? b) => new(A, b);
    }

    private readonly struct Pair
    {
        public readonly int A;
        public readonly int B;

        public Pair(int a, int b)
        {
            A = a;
            B = b;
        }
    } 
    #endregion
    
    public static IEnumerable<SubSequence> GetMatchingBlocks<T>(IList<T> left, IList<T> right) where T : IEquatable<T>
    {
        if (left == null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right == null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        var matches = RecurseMatches(left, right, 0, 0, left.Count, right.Count, 10);
        return CollapseSequences(matches);
    }

    private static IEnumerable<Pair> RecurseMatches<T>(
        IList<T> a, IList<T> b, int aLow, int bLow, int aHigh, int bHigh, int maxRecursion) where T : IEquatable<T>
    {
        while (true)
        {
            if (maxRecursion < 0 || aLow >= aHigh || bLow >= bHigh)
            {
                yield break;
            }

            var added = false;
            var lastAPos = aLow - 1;
            var lastBPos = bLow - 1;

            foreach (var pair in UniqueLcs(a, b, aLow, bLow, aHigh, bHigh))
            {
                var aPos = pair.A + aLow;
                var bPos = pair.B + bLow;

                if (lastAPos + 1 != aPos || lastBPos + 1 != bPos)
                {
                    foreach (var item in RecurseMatches(a, b, lastAPos + 1, lastBPos + 1, aPos, bPos, maxRecursion - 1))
                    {
                        yield return item;
                    }
                }

                lastAPos = aPos;
                lastBPos = bPos;
                added = true;
                yield return new Pair(aPos, bPos);
            }

            if (added)
            {
                aLow = lastAPos + 1;
                bLow = lastBPos + 1;
                maxRecursion -= 1;
                continue;
            }

            if (a[aLow].Equals(b[bLow]))
            {
                while (aLow < aHigh && bLow < bHigh && a[aLow].Equals(b[bLow]))
                {
                    yield return new Pair(aLow++, bLow++);
                }
                
                maxRecursion -= 1;
                continue;
            }

            if (a[aHigh - 1].Equals(b[bHigh - 1]))
            {
                var newAHigh = aHigh - 1;
                var newBHigh = bHigh - 1;

                while (newAHigh > aLow && newBHigh > bLow && a[newAHigh - 1].Equals(b[newBHigh - 1]))
                {
                    newAHigh--;
                    newBHigh--;
                }

                foreach (var item in RecurseMatches(a, b, lastAPos + 1, lastBPos + 1, newAHigh, newBHigh, maxRecursion - 1))
                {
                    yield return item;
                }

                for (var i = 0; i < aHigh - newAHigh; i++)
                {
                    yield return new Pair(newAHigh + i, newBHigh + i);
                }
            }

            break;
        }
    }

    private static IEnumerable<SubSequence> CollapseSequences(IEnumerable<Pair> list)
    {
        var startA = new int?();
        var startB = new int?();
        var length = 0;

        foreach (var pair in list)
        {
            var a = pair.A;
            var b = pair.B;

            if (startA.HasValue && a == startA + length && b == startB + length)
            {
                length += 1;
            }
            else
            {
                if (startA.HasValue)
                {
                    yield return new SubSequence(startA.Value, startB!.Value, length);
                }

                startA = a;
                startB = b;
                length = 1;
            }
        }

        if (length != 0)
        {
            yield return new SubSequence(startA!.Value, startB!.Value, length);
        }
    }

    private static IEnumerable<Pair> UniqueLcs<T>(
        IList<T> a, IList<T> b, int aLow, int bLow, int aHigh, int bHigh) where T : IEquatable<T>
    {
        var index = new Dictionary<T, NullablePair>(a.Count);
        var act = aHigh - aLow;
        var bct = bHigh - bLow;
        
        for (var i = 0; i < act; i++)
        {
            var line = a[aLow + i];
            index[line] = index.TryGetValue(line, out var value) ? value.SetA(null) : new NullablePair(i, null);
        }
        
        var btoa = new int?[bct];
        
        for (var i = 0; i < bct; i++)
        {
            var line = b[bLow + i];

            if (!index.TryGetValue(line, out var next) || !next.A.HasValue)
            {
                continue;
            }
            
            if (next.B.HasValue)
            {
                btoa[next.B.Value] = null;
                index[line] = index[line].SetA(null);
            }
            else
            {
                index[line] = index[line].SetB(i);
                btoa[i] = next.A.Value;
            }
        }

        var backPointers = new int?[bct];
        var stacksAndLasts = new List<Pair>();
        var k = 0;

        for (var bPos = 0; bPos < btoa.Length; bPos++)
        {
            var apos = btoa[bPos];
            
            if (!apos.HasValue)
            {
                continue;
            }

            if (stacksAndLasts.Count != 0 && stacksAndLasts[^1].A < apos.Value)
            {
                k = stacksAndLasts.Count;
            }
            else if (stacksAndLasts.Count != 0 && stacksAndLasts[k].A < apos &&
                     (k == stacksAndLasts.Count - 1 || stacksAndLasts[k + 1].A > apos))
            {
                k++;
            }
            else
            {
                k = BinarySearch(stacksAndLasts, apos.Value);
            }

            if (k > 0)
            {
                backPointers[bPos] = stacksAndLasts[k - 1].B;
            }

            if (k < stacksAndLasts.Count)
            {
                stacksAndLasts[k] = new Pair(apos.Value, bPos);
            }
            else
            {
                stacksAndLasts.Add(new Pair(apos.Value, bPos));
            }
        }

        if (stacksAndLasts.Count == 0)
        {
            return Enumerable.Empty<Pair>();
        }

        var j = new int?(stacksAndLasts[^1].B);

        k = stacksAndLasts.Count;

        while (j.HasValue)
        {
            stacksAndLasts[--k] = new Pair(btoa[j.Value]!.Value, j.Value);
            j = backPointers[j.Value];
        }

        return stacksAndLasts;
    }

    private static int BinarySearch(IReadOnlyList<Pair> pairs, int search)
    {
        var left = -1;
        var right = pairs.Count;

        while (left + 1 < right)
        {
            var middle = (left + right) / 2;
            
            if (pairs[middle].A > search)
            {
                right = middle;
            }
            else
            {
                left = middle;
            }
        }

        return left == -1 ? 0 : left;
    }
}