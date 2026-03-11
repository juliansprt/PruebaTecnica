using AuditoriaFase4.Application;
using AuditoriaFase4.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AuditoriaFase4.Tests;

/// <summary>
/// Cobertura superficial: solo camino feliz. No valida excepciones ni arquitectura.
/// </summary>
public class CreateOrderUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_CreaOrdenConItems()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .UseLazyLoadingProxies()
            .Options;
        await using var db = new AppDbContext(options);
        await db.Database.OpenConnectionAsync();
        await db.Database.EnsureCreatedAsync();

        var useCase = new CreateOrderUseCase(db);
        var order = await useCase.ExecuteAsync("Cliente Test", new List<(string, int, decimal)>
        {
            ("Producto A", 2, 10.50m),
            ("Producto B", 1, 25.00m)
        });

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal("Cliente Test", order.CustomerName);
        Assert.Equal(46.00m, order.Total);
        Assert.Equal(2, order.Items.Count);
    }
}
