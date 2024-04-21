﻿using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Options;

[UseInPython]
public class LayerConfiguration
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public Dictionary<string, object> Parameters { get; set; } = null!;

    public string GetString(string parameterName)
    {
        return Parameters[parameterName].ToString()!;
    }
    
    public int GetInt(string parameterName)
    {
        return int.Parse(GetString(parameterName));
    }
    
    public bool GetBool(string parameterName)
    {
        return bool.Parse(GetString(parameterName));
    }
}