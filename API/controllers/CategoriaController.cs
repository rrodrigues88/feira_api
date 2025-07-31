using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly FeiraDbContext _context;

    public CategoriaController(FeiraDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        => await _context.Categorias.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(string id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        return categoria == null ? NotFound() : categoria;
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Id))
        {
            categoria.Id = Guid.NewGuid().ToString();
        }

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(string id, Categoria categoria)
    {
        if (id != categoria.Id) return BadRequest();

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categorias.Any(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(string id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
