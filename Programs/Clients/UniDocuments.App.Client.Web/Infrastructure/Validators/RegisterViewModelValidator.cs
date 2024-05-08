using FluentValidation;
using UniDocuments.App.Client.Web.ViewModels.Account;

namespace UniDocuments.App.Client.Web.Infrastructure.Validators;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Пароли не совпадают");
    }
}