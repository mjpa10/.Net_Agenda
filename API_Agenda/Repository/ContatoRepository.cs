using API_Agenda.Models;
using API_Agenda.Pagination;
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
    public IEnumerable<Contato> GetAll()
    {
        return  _context.Contatos.ToList();
    }
    public async Task<Contato?> GetContatoAsync(int id)
    {
        return await _context.Contatos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Contato Create(Contato contato)
    {
        _context.Contatos.Add(contato);
        return contato;
    }
    public Contato Update(Contato contato)
    {      
        _context.Entry(contato).State = EntityState.Modified;

        return contato;
    }

    public Contato Delete(int id)
    {
        var contato = _context.Contatos.Find(id);

        if (contato == null)
            throw new ArgumentNullException(nameof(contato));

        _context.Contatos.Remove(contato);
        return contato;
    }

    public IEnumerable<Contato> GetContatos(ContatosParameters contatosParameters)
    {
        return GetAll()
            .OrderBy(c => c.Nome)
            .Skip((contatosParameters.PageNumber - 1)* contatosParameters.PageSize)
            .Take(contatosParameters.PageSize).ToList();
    }

}



