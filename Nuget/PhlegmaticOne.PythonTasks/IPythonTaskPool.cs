namespace PhlegmaticOne.PythonTasks;

public interface IPythonTaskPool
{
    int QueueCount { get; }
    void Start(CancellationToken cancellationToken);
    void Enqueue(PythonTask task);
}