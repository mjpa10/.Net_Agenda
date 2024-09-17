using API_Agenda.Models;
using API_Agenda.Pagination;
using X.PagedList;

namespace API_Agenda.Repository;

public interface IContatoRepository
{

    Task<IPagedList<Contato>> GetContatosAsync(ContatosParameters contatosParams, string? searchTerm = null);
    Task<IEnumerable<Contato>> GetAllAsync();
    Task<Contato?> GetContatoAsync(int id);
    Contato Create(Contato contato);
    Contato Update(Contato contato);
    Contato Delete(int id);
}
