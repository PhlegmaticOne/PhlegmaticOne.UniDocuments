using Newtonsoft.Json;

var data = new HashSet<string>()
{
    "a", "b", "v"
};

var json = JsonConvert.SerializeObject(data);

var back = JsonConvert.DeserializeObject<HashSet<string>>(json);

Console.WriteLine("Hellow");


