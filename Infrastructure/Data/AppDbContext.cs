using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    var tenantId = _tenantProvider.GetTenantId();

   
    modelBuilder.Entity<Invoice>()
        .HasQueryFilter(x => x.TenantId == tenantId);

    
    modelBuilder.Entity<Invoice>()
        .Property(x => x.Total)
        .HasPrecision(18, 2);

    base.OnModelCreating(modelBuilder);
}
}