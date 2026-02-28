namespace Application.Interfaces;

public interface ITenantProvider
{
    Guid GetTenantId();
    string? GetUsername();
    string? GetRole();
}