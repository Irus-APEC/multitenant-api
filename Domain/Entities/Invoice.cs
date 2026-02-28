namespace Domain.Entities;

public class Invoice : BaseEntity
{
    public string Number { get; set; } = "";
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}