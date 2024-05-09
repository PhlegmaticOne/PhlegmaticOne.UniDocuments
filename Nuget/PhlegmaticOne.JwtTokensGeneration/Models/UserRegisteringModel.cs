namespace PhlegmaticOne.JwtTokensGeneration.Models;

public record UserRegisteringModel(
    Guid Id, int StudyRole, int AppRole, string FirstName, string LastName, string UserName);