namespace Core.Entities;

public class Venda
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ProdutoId { get; set; }
    public Produto? Produto { get; set; } 
    public int Quantidade { get; set; }
    public required string Data { get; set; } = DateTime.UtcNow.ToString("o");
    public required string FeiranteId { get; set; }
    public Feirante? Feirante { get; set; } 
}