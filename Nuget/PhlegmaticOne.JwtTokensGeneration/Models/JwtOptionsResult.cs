namespace PhlegmaticOne.JwtTokensGeneration.Models;

public class JwtOptionsResult
{
    public string Token { get; }
    public int TimeInMinutes { get; }

    public JwtOptionsResult(string token, int timeInMinutes)
    {
        Token = token;
        TimeInMinutes = timeInMinutes;
    }
}