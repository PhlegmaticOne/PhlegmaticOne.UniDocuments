using PhlegmaticOne.PythonTasks;

var token = CancellationToken.None;
PythonTaskPool.CreateAndStart(new [] { "custom_model" }.ToScriptNamesProvider(), token);

Console.WriteLine("Completed");