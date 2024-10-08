﻿using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Models.Inferring;
using UniDocuments.Text.Services.Neural.Common;

namespace UniDocuments.Text.Services.Neural.Keras.Tasks;

public class PythonTaskInferKerasModel : PythonTask<InferVectorInput, InferVectorOutput>
{
    private readonly int _paragraphId;
    
    public PythonTaskInferKerasModel(InferVectorInput input, int paragraphId) : base(input)
    {
        _paragraphId = paragraphId;
    }

    public override string ScriptName => "keras2vec";
    public override string MethodName => "infer";
    protected override InferVectorOutput MapResult(dynamic result)
    {
        var entries = new List<InferVectorEntry>();
        
        foreach (var infer in result)
        {
            var paragraphId = int.Parse(((object)infer.index).ToString()!);
            var similarity = double.Parse(((object)infer.cos).ToString()!);
            entries.Add(new InferVectorEntry(paragraphId, similarity));
        }
        
        return new InferVectorOutput(_paragraphId, entries);
    }
}