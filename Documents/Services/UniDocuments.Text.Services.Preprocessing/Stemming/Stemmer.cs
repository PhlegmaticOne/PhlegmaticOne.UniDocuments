using System.Text.RegularExpressions;
using UniDocuments.Text.Domain.Services.Preprocessing.Stemmer;

namespace UniDocuments.Text.Services.Preprocessing.Stemming;

public class Stemmer : IStemmer
{
    public string Stem(string word)
    {
        var m = StemmingRegexes.RvreRegex().Matches(word);

        if (m.Count <= 0)
        {
            return word;
        }

        var match = m[0];
        var groupCollection = match.Groups;
        var pre = groupCollection[1].ToString();
        var rv = groupCollection[2].ToString();
        var temp = StemmingRegexes.PerfectiveGroundRegex().Matches(rv);
        var stringTemp = ReplaceFirst(temp, rv);

        if (stringTemp.Equals(rv))
        {
            var tempRV = StemmingRegexes.ReflectiveRegex().Matches(rv);
            rv = ReplaceFirst(tempRV, rv);
            temp = StemmingRegexes.AdjectiveRegex().Matches(rv);
            stringTemp = ReplaceFirst(temp, rv);
            
            if (!stringTemp.Equals(rv))
            {
                rv = stringTemp;
                tempRV = StemmingRegexes.ParticipleRegex().Matches(rv);
                rv = ReplaceFirst(tempRV, rv);
            }
            else
            {
                temp = StemmingRegexes.VerbRegex().Matches(rv);
                stringTemp = ReplaceFirst(temp, rv);

                if (stringTemp.Equals(rv))
                {
                    tempRV = StemmingRegexes.NounRegex().Matches(rv);
                    rv = ReplaceFirst(tempRV, rv);
                }
                else
                {
                    rv = stringTemp;
                }
            }
        }
        else
        {
            rv = stringTemp;
        }

        var tempRv = StemmingRegexes.IRegex().Matches(rv);
        rv = ReplaceFirst(tempRv, rv);

        if (StemmingRegexes.DerivationalRegex().Matches(rv).Count > 0)
        {
            tempRv = StemmingRegexes.DerRegex().Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
        }

        temp = StemmingRegexes.PRegex().Matches(rv);
        stringTemp = ReplaceFirst(temp, rv);

        if (stringTemp.Equals(rv))
        {
            tempRv = StemmingRegexes.SuperlativeRegex().Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
            tempRv = StemmingRegexes.NnRegex().Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
        }
        else
        {
            rv = stringTemp;
        }

        return pre + rv;
    }

    private static string ReplaceFirst(MatchCollection collection, string part)
    {
        if (collection.Count == 0)
        {
            return part;
        }

        var temp = part;

        for (var i = 0; i < collection.Count; i++)
        {
            var groupCollection = collection[i].Groups;
            var c = groupCollection[i].ToString();

            if (temp.Contains(c))
            {
                temp = temp.Replace(c, "");
            }
        }

        return temp;
    }
}