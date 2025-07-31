namespace Core.Entities;

public class Produto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }

    // Relações (adicionar essas propriedades)
    public required string FeiranteId { get; set; }
    public Feirante? Feirante { get; set; }

    public required string CategoriaId { get; set; }
    public Categoria? Categoria { get; set; } 
}