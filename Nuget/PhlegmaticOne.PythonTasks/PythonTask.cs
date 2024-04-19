namespace PhlegmaticOne.PythonTasks;

public abstract class PythonTask
{
    public static IPythonTaskPool? TaskPool { get; set; }
    public object Input { get; }
    public abstract string ScriptName { get; }
    public abstract string MethodName { get; }
    protected PythonTask(object input) => Input = input;
    public abstract void OnComplete(dynamic result);
}

public abstract class PythonTask<TIn, TOut> : PythonTask
{
    private TaskCompletionSource<TOut?>? _completionSource;
    private bool _isCompleted;

    public TOut? Result { get; protected set; }

    protected PythonTask(TIn input) : base(input!)
    {
        _completionSource = new TaskCompletionSource<TOut?>();
    }

    public Task<TOut?> Execute(IPythonTaskPool taskPool)
    {
        if (_isCompleted)
        {
            return Task.FromResult(Result);
        }
        
        taskPool.Enqueue(this);
        return AwaitExecutionComplete();
    }

    public override void OnComplete(dynamic result)
    {
        _isCompleted = true;
        Result = MapResult(result);
        _completionSource?.TrySetResult(Result);
        _completionSource = null;
    }

    protected abstract TOut MapResult(dynamic result);

    private Task<TOut?> AwaitExecutionComplete()
    {
        if (_completionSource is null)
        {
            return Task.FromResult(Result);
        }
        
        return _completionSource.Task;
    }
}