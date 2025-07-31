using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly FeiraDbContext _context;

    public ProdutoController(FeiraDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        => await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Feirante)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(string id)
        => await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Feirante)
            .FirstOrDefaultAsync(p => p.Id == id)
            is var produto && produto == null ?
                NotFound() : produto;

    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        if (string.IsNullOrWhiteSpace(produto.Id))
        {
            produto.Id = Guid.NewGuid().ToString();
        }

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(string id, Produto produto)
    {
        if (id != produto.Id) return BadRequest();
        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(string id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return NotFound();

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
