using API_Agenda.Models;

namespace API_Agenda.Services;

public interface IValidadorContato
{
    public Task<Dictionary<string, string>> ValidarContatoAsync(Contato contato, int id);
}
