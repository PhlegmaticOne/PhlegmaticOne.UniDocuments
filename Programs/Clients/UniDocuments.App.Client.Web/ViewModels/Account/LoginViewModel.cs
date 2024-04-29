using System.ComponentModel.DataAnnotations;
using UniDocuments.App.Client.Web.ViewModels.Base;

namespace UniDocuments.App.Client.Web.ViewModels.Account;

public class LoginViewModel : ErrorHavingViewModel
{
    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.Password)] public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}