using System.Text.RegularExpressions;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Text.Stemming;

public static partial class StemmingRegexes
{
    [GeneratedRegex("((ив|ивши|ившись|ыв|ывши|ывшись)|((<;=[ая])(в|вши|вшись)))$")]
    public static partial Regex PerfectiveGroundRegex();

    [GeneratedRegex("(с[яь])$")]
    public static partial Regex ReflectiveRegex();
    [GeneratedRegex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$")]
    public static partial Regex AdjectiveRegex();
    [GeneratedRegex("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$")]
    public static partial Regex ParticipleRegex();
    [GeneratedRegex("((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$")]
    public static partial Regex VerbRegex();
    [GeneratedRegex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$")]
    public static partial Regex NounRegex();
    [GeneratedRegex("^(.*?[аеиоуыэюя])(.*)$")]
    public static partial Regex RvreRegex();
    [GeneratedRegex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$")]
    public static partial Regex DerivationalRegex();
    [GeneratedRegex("ость?$")]
    public static partial Regex DerRegex();
    [GeneratedRegex("(ейше|ейш)$")]
    public static partial Regex SuperlativeRegex();
    [GeneratedRegex("и$")]
    public static partial Regex IRegex();
    [GeneratedRegex("ь$")]
    public static partial Regex PRegex();
    [GeneratedRegex("нн$")]
    public static partial Regex NnRegex();
}
