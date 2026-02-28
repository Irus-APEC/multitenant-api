namespace Application.Interfaces;

public interface IUserService
{
    (bool ok, string? role, Guid tenantId) ValidateCredentials(string username, string password);
    bool ChangePassword(string username, string currentPassword, string newPassword);
    bool ForgotPassword(string emailOrUsername, out string resetCode);
}