using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeiranteController : ControllerBase
{
    private readonly FeiraDbContext _context;

    public FeiranteController(FeiraDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feirante>>> GetFeirantes()
        => await _context.Feirantes.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Feirante>> GetFeirante(string id)
        => await _context.Feirantes.FindAsync(id) is var feirante && feirante == null ?
            NotFound() : feirante;

    [HttpPost]
    public async Task<ActionResult<Feirante>> PostFeirante(Feirante feirante)
    {
        if (string.IsNullOrWhiteSpace(feirante.Id))
        {
            feirante.Id = Guid.NewGuid().ToString();
        }

        _context.Feirantes.Add(feirante);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFeirante), new { id = feirante.Id }, feirante);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFeirante(string id, Feirante feirante)
    {
        if (id != feirante.Id) return BadRequest();
        _context.Entry(feirante).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeirante(string id)
    {
        var feirante = await _context.Feirantes.FindAsync(id);
        if (feirante == null) return NotFound();

        _context.Feirantes.Remove(feirante);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
