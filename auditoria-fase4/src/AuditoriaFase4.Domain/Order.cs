namespace AuditoriaFase4.Domain;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
