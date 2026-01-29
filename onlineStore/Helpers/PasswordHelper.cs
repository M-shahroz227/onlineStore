using Microsoft.AspNetCore.Identity;
using onlineStore.Model;

public static class PasswordHelper
{
    public static string Hash(string password)
    {
        var hasher = new PasswordHasher<User>();
        return hasher.HashPassword(null!, password);
    }
}
