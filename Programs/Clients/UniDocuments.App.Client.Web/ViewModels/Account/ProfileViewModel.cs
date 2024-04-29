namespace UniDocuments.App.Client.Web.ViewModels.Account;

public class ProfileViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime JoinDate { get; set; }
    public byte[] AvatarData { get; set; } = null!;
}