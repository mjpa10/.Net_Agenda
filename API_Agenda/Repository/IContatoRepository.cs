using API_Agenda.Models;
using System.Linq.Expressions;

namespace API_Agenda.Repository;

public interface IContatoRepository
{
    Task<IEnumerable<Contato>> GetAllAsync();
    Task<Contato?> GetContatoAsync(int id);
    Contato Create(Contato contato);
    Contato Update(Contato contato);
    Contato Delete(int id);
}
