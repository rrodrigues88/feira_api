using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendaController : ControllerBase
{
    private readonly FeiraDbContext _context;

    public VendaController(FeiraDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Venda>>> GetVendas()
        => await _context.Vendas
            .Include(v => v.Produto)
            .Include(v => v.Feirante)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Venda>> GetVenda(string id)
        => await _context.Vendas
            .Include(v => v.Produto)
            .Include(v => v.Feirante)
            .FirstOrDefaultAsync(v => v.Id == id)
            is var venda && venda == null ?
                NotFound() : venda;

    [HttpPost]
    public async Task<ActionResult<Venda>> PostVenda(Venda venda)
    {
        if (string.IsNullOrWhiteSpace(venda.Id))
        {
            venda.Id = Guid.NewGuid().ToString();
        }

        var produto = await _context.Produtos.FindAsync(venda.ProdutoId);
        if (produto == null || produto.Quantidade < venda.Quantidade)
        {
            return BadRequest("Produto não encontrado ou estoque insuficiente.");
        }

        produto.Quantidade -= venda.Quantidade;

        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVenda), new { id = venda.Id }, venda);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVenda(string id, Venda venda)
    {
        if (id != venda.Id) return BadRequest();
        _context.Entry(venda).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenda(string id)
    {
        var venda = await _context.Vendas
            .Include(v => v.Produto)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null || venda.Produto == null)
            return NotFound();

        venda.Produto.Quantidade += venda.Quantidade;
        _context.Vendas.Remove(venda);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
