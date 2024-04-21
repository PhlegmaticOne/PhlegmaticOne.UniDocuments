var result = new int[5]
{
    5, 66, 123, 543, 67
};

Array.Sort(result, (a, b) => a.CompareTo(b));

foreach (var i in result)
{
    Console.WriteLine(i);
}