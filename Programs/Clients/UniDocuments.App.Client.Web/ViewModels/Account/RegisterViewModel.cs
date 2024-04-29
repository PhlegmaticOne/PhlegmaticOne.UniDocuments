using System.ComponentModel.DataAnnotations;
using UniDocuments.App.Client.Web.ViewModels.Base;

namespace UniDocuments.App.Client.Web.ViewModels.Account;

public class RegisterViewModel : ErrorHavingViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.Password)] public string Password { get; set; } = null!;

    [DataType(DataType.Password)] public string ConfirmPassword { get; set; } = null!;
}