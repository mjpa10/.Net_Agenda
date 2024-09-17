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
    public async Task<IEnumerable<Contato>> GetAllAsync()
    {
        return  await _context.Contatos.ToListAsync();
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

    public async Task<PagedList<Contato>> GetContatosAsync(ContatosParameters contatosParameters, string? searchTerm = null)
    {
        var contatos = await GetAllAsync();

        var contatosOrdenados = contatos.OrderBy(c => c.Nome).AsQueryable();

        // Se o parâmetro de busca (searchTerm) não for nulo ou vazio, aplica o filtro.
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            contatosOrdenados = contatosOrdenados.Where(c => c.Nome.Contains(searchTerm) || // Filtro por nome
                                                  c.Email.Contains(searchTerm) || // Filtro por e-mail
                                                  c.Telefone.Contains(searchTerm)); // Filtro por telefone
        }
        // Aplica a paginação aos contatos filtrados e ordenados.
        var resultado = PagedList<Contato>.ToPagedList(contatosOrdenados, contatosParameters.PageNumber, contatosParameters.PageSize);
        return resultado;
    }

}



