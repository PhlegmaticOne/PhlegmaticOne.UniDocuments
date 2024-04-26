﻿using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Keras.Core.Models;

namespace UniDocuments.Text.Services.Neural.Keras.Core.Tasks;

public class PythonTaskLoadKerasModel : PythonTask<string, KerasManagedModel>
{
    public PythonTaskLoadKerasModel(string input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "load";
    protected override KerasManagedModel MapResult(dynamic result)
    {
        return new KerasManagedModel(result);
    }
}