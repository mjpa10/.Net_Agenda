using API_Agenda.Models;
using API_Agenda.Services;
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace API_Agenda.Repository;

public class ContatoRepository : IContatoRepository
{
    private readonly AppDbContext _context;
   

    public ContatoRepository(AppDbContext context)
    {
        _context = context;
       
    }
    public async Task<IEnumerable<Contato>> GetAllAsync()
    {
        return await _context.Contatos.ToListAsync();
    }
    public async Task<Contato?> GetContatoAsync(int id)
    {
        return await _context.Contatos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Contato Create(Contato contato)
    {
        if (contato == null)
            throw new ArgumentNullException(nameof(contato));

        _context.Contatos.Add(contato);
        _context.SaveChanges();
        return contato;
    }
    public Contato Update(Contato contato)
    {
        if(contato == null)
            throw new ArgumentNullException(nameof(contato));

        _context.Entry(contato).State = EntityState.Modified;
        _context.SaveChanges();

        return contato;
    }

    public Contato Delete(int id)
    {
        var contato = _context.Contatos.Find(id);

        if (contato == null)
            throw new ArgumentNullException(nameof(contato));

        _context.Contatos.Remove(contato);
        _context.SaveChanges();
        return contato;
    }

}



