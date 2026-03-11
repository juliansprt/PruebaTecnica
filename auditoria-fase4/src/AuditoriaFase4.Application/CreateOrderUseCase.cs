using AuditoriaFase4.Domain;
using AuditoriaFase4.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AuditoriaFase4.Application;

/// <summary>
/// ANTIPATRÓN: Caso de uso en capa Application que inyecta DbContext.
/// Rompe Clean Architecture y dificulta pruebas unitarias.
/// </summary>
public class CreateOrderUseCase
{
    private readonly AppDbContext _db;

    public CreateOrderUseCase(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Order> ExecuteAsync(string customerName, List<(string productName, int quantity, decimal unitPrice)> items)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            CreatedAt = DateTime.UtcNow,
            Total = 0
        };
        foreach (var (productName, quantity, unitPrice) in items)
        {
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice
            });
            order.Total += quantity * unitPrice;
        }
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }
}
