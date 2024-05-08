namespace PhlegmaticOne.JwtTokensGeneration.Models;

public record UserRegisteringModel(
    Guid Id, int Role, string FirstName, string LastName, string UserName);