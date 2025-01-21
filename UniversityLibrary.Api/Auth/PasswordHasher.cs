using System.Security.Cryptography;

namespace UniversityLibrary.Api.Auth;

public static class PasswordHasher
{
    private const int IterationCount = 10000;
    private const int HashSize = 256 / 8;

    public static string HashPassword(string password, string salt)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrEmpty(salt))
            throw new ArgumentNullException(nameof(salt));

        byte[] saltBytes = Convert.FromBase64String(salt);

        using (var pbkdf2 = new Rfc2898DeriveBytes(
                   password,
                   saltBytes,
                   IterationCount,
                   HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(HashSize);
            return Convert.ToBase64String(hash);
        }
    }

    public static bool VerifyPassword(string hashedPassword, string providedPassword, string salt)
    {
        if (string.IsNullOrEmpty(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword));
        if (string.IsNullOrEmpty(providedPassword))
            throw new ArgumentNullException(nameof(providedPassword));
        if (string.IsNullOrEmpty(salt))
            throw new ArgumentNullException(nameof(salt));

        string computedHash = HashPassword(providedPassword, salt);

        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedHash),
            Convert.FromBase64String(hashedPassword));
    }

    public static string GenerateSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }
}