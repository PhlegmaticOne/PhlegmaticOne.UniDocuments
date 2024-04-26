﻿using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Keras.Core.Tasks;

[UseInPython]
public class SaveKerasModelInput
{
    public SaveKerasModelInput(string path, dynamic model)
    {
        Path = path;
        Model = model;
    }

    public string Path { get; }
    public dynamic Model { get; }
}

public class PythonTaskSaveKerasModel : PythonUnitTask<SaveKerasModelInput>
{
    public PythonTaskSaveKerasModel(SaveKerasModelInput input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "save";
}