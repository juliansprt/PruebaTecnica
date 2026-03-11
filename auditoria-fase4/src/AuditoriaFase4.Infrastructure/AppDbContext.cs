using AuditoriaFase4.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuditoriaFase4.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Lazy loading habilitado: provoca N+1 si se accede a Items en un bucle sin Include.
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Items).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
        });
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(x => x.Id);
        });
    }
}
