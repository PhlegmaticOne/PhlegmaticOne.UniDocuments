using System.Security.Cryptography;
using System.Text;

namespace PhlegmaticOne.PasswordHasher.Implementation;

public class SecurePasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return HashPrivate(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return HashPrivate(password) == hashedPassword;
    }

    private static string HashPrivate(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.Default.GetBytes(password);
        var hashed = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashed);
    }
}