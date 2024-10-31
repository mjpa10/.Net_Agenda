using API_Agenda.Models;
using API_Agenda.Pagination;
using API_Agenda.Services;
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


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

        _context.Contatos.Remove(contato);
        return contato;
    }

    public async Task<IPagedList<Contato>> GetContatosAsync(ContatosParameters contatosParameters, string? searchTerm = null)
    {
        var contatos = await GetAllAsync();

        var contatosOrdenados = contatos.OrderBy(c => c.Nome).AsQueryable();

        // Se o parâmetro de busca (searchTerm) não for nulo ou vazio, aplica o filtro.
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            //Este método procura a substring(searchTerm) em cada campo(Nome, Email, Telefone) do contato.
            // O método IndexOf é usado para localizar a substring, ignorando se as letras são maiúsculas ou minúsculas.
            // Retorna um índice maior ou igual a 0 se a substring for encontrada em qualquer posição do texto.
            // Caso contrário, retorna -1.
            contatosOrdenados = contatosOrdenados.Where(c =>
                  c.Nome.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 || // Filtro por nome
                  c.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 || // Filtro por e-mail
                  c.Telefone.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0); // Filtro por telefone
        }

        // Aplica a paginação aos contatos filtrados e ordenados.
        var resultado = await contatosOrdenados.ToPagedListAsync(contatosParameters.PageNumber,
                                                        contatosParameters.PageSize);
        return resultado;
    }

}



