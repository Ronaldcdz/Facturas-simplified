using Facturas_simplified.Abstractions;
using Facturas_simplified.Clients;
using Facturas_simplified.Invoices;
using Facturas_simplified.Ncfs;
using Facturas_simplified.Payments;
using Facturas_simplified.Provinces;
using Facturas_simplified.Services;
using Microsoft.EntityFrameworkCore;

namespace Facturas_simplified.Database;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {

  }
  public DbSet<Client> Clients { get; set; }
  public DbSet<Province> Provinces { get; set; }
  public DbSet<Invoice> Invoices { get; set; }
  public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
  public DbSet<Payment> Payments { get; set; }
  public DbSet<Ncf> Ncfs { get; set; }
  public DbSet<NcfRange> NcfRanges { get; set; }
  public DbSet<Service> Services { get; set; }

  public override int SaveChanges()
  {
    UpdateAuditFields();
    return base.SaveChanges();
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    UpdateAuditFields();
    return base.SaveChangesAsync(cancellationToken);
  }

  private void UpdateAuditFields()
  {
    var entries = ChangeTracker.Entries<AuditableEntity>();

    foreach (var entry in entries)
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedBy = "TheCreateUser";
        entry.Entity.CreatedOn = DateTime.UtcNow;
      }

      if (entry.State == EntityState.Modified)
      {
        entry.Entity.LastModifiedBy = "TheUpdateUser";
        entry.Entity.LastModifiedOn = DateTime.UtcNow;
      }
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Client>().HasOne(c => c.Province).WithMany(p => p.Clients).HasForeignKey(c => c.ProvinceId);
    modelBuilder.Entity<Invoice>()
      .HasOne(i => i.Ncf)
      .WithOne(n => n.Invoice)
      .HasForeignKey<Invoice>(i => i.NcfId);
  }
}
