﻿using System.Runtime.CompilerServices;

namespace PhlegmaticOne.PythonTasks;

public static class PythonTaskExtensions
{
    public static IPythonScriptNamesProvider ToScriptNamesProvider(this IEnumerable<string> names)
    {
        return new PythonScriptNamesProviderInternal(names.ToArray());
    }
    
    public static TaskAwaiter<TOut?> GetAwaiter<TIn, TOut>(this PythonTask<TIn, TOut> pythonTask)
    {
        var taskPool = PythonTask.TaskPool;

        if (taskPool is null)
        {
            throw new ArgumentException("TaskPool is null");
        }
        
        return pythonTask.ExecuteOnPool(taskPool).GetAwaiter();
    }
}