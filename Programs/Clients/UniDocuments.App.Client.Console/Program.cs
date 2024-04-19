using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Client.Console;

var source = new CancellationTokenSource();
PythonTaskPool.CreateAndStart(new PythonScriptNamesProvider(), source.Token);
var result = await new PythonTaskDivideNumber(20);
Console.WriteLine(result);
source.Cancel();