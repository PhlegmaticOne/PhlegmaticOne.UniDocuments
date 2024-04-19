namespace PhlegmaticOne.PythonTasks;

public interface IPythonTaskPool
{
    void Start(CancellationToken cancellationToken);
    void Enqueue(PythonTask task);
}