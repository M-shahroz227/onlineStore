using Microsoft.AspNetCore.Identity;
using onlineStore.Model;

public static class PasswordHelper
{
    private static readonly PasswordHasher<User> hasher = new PasswordHasher<User>();

    // 🔹 Hash password for storing in DB
    public static string Hash(string password)
    {
        // User instance can be null here
        return hasher.HashPassword(null!, password);
    }

    // 🔹 Verify password during login
    public static bool Verify(string password, string passwordHash)
    {
        // Check password against hash
        var result = hasher.VerifyHashedPassword(null!, passwordHash, password);

        return result == PasswordVerificationResult.Success
            || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
