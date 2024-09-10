using API_Agenda.Models;

namespace API_Agenda.Services;

public interface IValidadorContato
{
    public Task<List<string>> ValidarContatoAsync(Contato contato);
}
