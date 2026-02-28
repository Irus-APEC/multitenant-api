using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Serilog;
using UserEntity = Domain.Entities.User;

namespace Application.Services;

public class UserService : IUserService
{
    // Usuarios simulados (2 tenants distintos)
    private static readonly List<UserEntity> _users = new()
    {
        new UserEntity
        {
            Username = "admin",
            Email = "admin@demo.com",
            Role = "Admin",
            TenantId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            PasswordHash = Hash("1234")
        },
        new UserEntity
        {
            Username = "user1",
            Email = "user1@demo.com",
            Role = "User",
            TenantId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            PasswordHash = Hash("1234")
        }
    };

    public (bool ok, string? role, Guid tenantId) ValidateCredentials(string username, string password)
    {
        var u = _users.SingleOrDefault(x => x.Username == username);
        if (u is null) return (false, null, Guid.Empty);

        var ok = u.PasswordHash == Hash(password);
        return (ok, u.Role, u.TenantId);
    }

    public bool ChangePassword(string username, string currentPassword, string newPassword)
    {
        var u = _users.SingleOrDefault(x => x.Username == username);
        if (u is null) return false;

        if (u.PasswordHash != Hash(currentPassword)) return false;

        u.PasswordHash = Hash(newPassword);
        return true;
    }

    public bool ForgotPassword(string emailOrUsername, out string resetCode)
    {
        resetCode = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();

        Log.Information("OlvideMiClave solicitado para {UserOrEmail}. ResetCode={ResetCode}",
            emailOrUsername, resetCode);

        return true;
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}