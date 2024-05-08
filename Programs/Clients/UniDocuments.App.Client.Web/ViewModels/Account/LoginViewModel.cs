using System.ComponentModel.DataAnnotations;
using UniDocuments.App.Client.Web.ViewModels.Base;

namespace UniDocuments.App.Client.Web.ViewModels.Account;

public class LoginViewModel : ErrorHavingViewModel
{
    public string UserName { get; set; } = null!;
    [DataType(DataType.Password)] public string Password { get; set; } = null!;
    public string? ReturnUrl { get; set; }
}