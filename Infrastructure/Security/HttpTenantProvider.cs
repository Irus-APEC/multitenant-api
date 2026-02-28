using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

public class HttpTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _http;

    public HttpTenantProvider(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Guid GetTenantId()
    {
        var claim = _http.HttpContext?.User?.FindFirst("tenantId")?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }

    public string? GetUsername() =>
    _http.HttpContext?.User?.Identity?.Name
    ?? _http.HttpContext?.User?.FindFirst("Username")?.Value
    ?? _http.HttpContext?.User?.FindFirst("username")?.Value;

    public string? GetRole() =>
    _http.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value
    ?? _http.HttpContext?.User?.FindFirst("Role")?.Value;
}