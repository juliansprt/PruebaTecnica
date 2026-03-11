using AuditoriaFase4.Application;
using AuditoriaFase4.Domain;
using AuditoriaFase4.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuditoriaFase4.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly CreateOrderUseCase _createOrderUseCase;

    public OrdersController(AppDbContext db, CreateOrderUseCase createOrderUseCase)
    {
        _db = db;
        _createOrderUseCase = createOrderUseCase;
    }

    /// <summary>
    /// ANTIPATRÓN N+1: Se cargan órdenes sin Include; al iterar y acceder a .Items
    /// se dispara una consulta por cada orden (lazy loading).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        try
        {
            var orders = await _db.Orders.ToListAsync();
            var result = new List<OrderDto>();
            foreach (var order in orders)
            {
                // N+1: cada acceso a order.Items ejecuta una consulta SQL adicional.
                var itemDtos = order.Items.Select(i => new OrderItemDto(i.ProductName, i.Quantity, i.UnitPrice)).ToList();
                result.Add(new OrderDto(order.Id, order.CustomerName, order.Total, order.CreatedAt, itemDtos));
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// ANTIPATRÓN: Lógica de negocio (cálculo de total, validación) en el controlador.
    /// Además, try-catch repetitivo en lugar de middleware global.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            if (request.Items == null || request.Items.Count == 0)
                return BadRequest("Debe haber al menos un ítem.");
            try
            {
                foreach (var item in request.Items)
                {
                    if (item.Quantity <= 0 || item.UnitPrice < 0)
                        return BadRequest("Cantidad y precio deben ser positivos.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            var order = await _createOrderUseCase.ExecuteAsync(
                request.CustomerName,
                request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice)).ToList());
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public record OrderDto(Guid Id, string CustomerName, decimal Total, DateTime CreatedAt, List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);

public record CreateOrderRequest(string CustomerName, List<CreateOrderItemRequest> Items);
public record CreateOrderItemRequest(string ProductName, int Quantity, decimal UnitPrice);
