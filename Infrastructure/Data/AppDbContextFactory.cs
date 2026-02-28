using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Lee la conexión desde variable de entorno (Railway) o usa una local de fallback
        var conn =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? Environment.GetEnvironmentVariable("DefaultConnection")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=MultiTenantApiDb;Trusted_Connection=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(conn);

        // TenantProvider dummy (design-time)
        var tenantProvider = new DesignTimeTenantProvider();

        return new AppDbContext(optionsBuilder.Options, tenantProvider);
    }
}

internal class DesignTimeTenantProvider : Application.Interfaces.ITenantProvider
{
    public Guid GetTenantId() => Guid.Empty;
    public string? GetUsername() => null;
    public string? GetRole() => null;
}