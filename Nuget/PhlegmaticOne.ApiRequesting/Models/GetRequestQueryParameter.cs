namespace PhlegmaticOne.ApiRequesting.Models;

public class GetRequestQueryParameter
{
    private const string Equal = "=";
    private readonly string _parameterName;
    private readonly object _parameterValue;

    public GetRequestQueryParameter(string parameterName, object parameterValue)
    {
        _parameterName = parameterName;
        _parameterValue = parameterValue;
    }

    public string BuildQueryPart()
    {
        return string.Concat(_parameterName, Equal, _parameterValue.ToString());
    }
}