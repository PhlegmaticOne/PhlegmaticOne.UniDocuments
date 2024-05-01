using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Common;

[UseInPython]
public interface IInferOptions
{
    string TokenizeRegex { get; }
}