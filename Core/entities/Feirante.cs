namespace Core.Entities;

public class Feirante
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public required string Nome { get; set; }
    public required string Contato { get; set; }
}