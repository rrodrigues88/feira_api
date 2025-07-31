namespace Core.Entities;

public class Categoria
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public required string Nome { get; set; }
}