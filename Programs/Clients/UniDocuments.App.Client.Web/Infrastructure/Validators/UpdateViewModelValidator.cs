using FluentValidation;
using UniDocuments.App.Client.Web.ViewModels.Account;

namespace UniDocuments.App.Client.Web.Infrastructure.Validators;

public class UpdateViewModelValidator : AbstractValidator<UpdateAccountViewModel>
{
    public UpdateViewModelValidator()
    {
        RuleFor(x => x.NewPasswordConfirm)
            .Equal(x => x.NewPassword)
            .When(x => string.IsNullOrEmpty(x.OldPassword) == false)
            .WithMessage("Пароли не равны");
    }
}