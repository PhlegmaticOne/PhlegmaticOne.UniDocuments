using Newtonsoft.Json;

var set = new HashSet<uint> { uint.MaxValue, 534534535 };
var json = JsonConvert.SerializeObject(set);
var test = JsonConvert.DeserializeObject<HashSet<uint>>(json);

Console.WriteLine(test.First());