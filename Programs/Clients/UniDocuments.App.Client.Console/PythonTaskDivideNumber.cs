using PhlegmaticOne.PythonTasks;

namespace UniDocuments.App.Client.Console;

public class PythonTaskDivideNumber : PythonTask<int, int>
{
    public PythonTaskDivideNumber(int input) : base(input) { }

    public override string ScriptName => "divider";
    public override string MethodName => "test";
    
    protected override int MapResult(dynamic result)
    {
        return (int)result;
    }
}