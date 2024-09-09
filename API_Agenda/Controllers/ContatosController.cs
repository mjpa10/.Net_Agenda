using API_Agenda.Models;
using APIAgenda.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Agenda.Controllers;

[ApiController]
[Route("[controller]")]
public class ContatosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContatosController(AppDbContext context)
    { 
        _context = context; 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contato>>> GetAsync()
    {
        var contatos = _context.Contatos;

        if (contatos == null)
            return NotFound();

        var listaContatos = await contatos.ToListAsync();

        return contatos;
    }

    [HttpGet("{id:int}", Name="ObterContato")]
    public async Task<ActionResult<Contato>> GetByIdAsync(int id)
    {
        var contato = await _context.Contatos.FirstOrDefaultAsync(p => p.Id == id);
       
        if (contato == null )
            return NotFound();

        return contato;
    }

    [HttpPost]
    public async Task<ActionResult<Contato>> PostAsync(Contato contato)
    {
        if (contato == null)
            return BadRequest("Contato Inválido");

        _context.Contatos.Add(contato);

        await _context.SaveChangesAsync();

        return new CreatedAtRouteResult("ObterContato",
            new { id = contato.Id }, contato);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Contato>> PutAsync(int id, Contato contato)
    {
        if (id != contato.Id)
            return BadRequest();

        var ContatoAtualizado = _context.Entry(contato).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Ok(ContatoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Contato>> Delete(int id)
    {
        var contato = await _context.Contatos.FindAsync(id);

        if (contato is null)
            return NotFound("contato não encontrado...");

        _context.Remove(contato);
        await _context.SaveChangesAsync();

        return Ok(contato);
    }
}
