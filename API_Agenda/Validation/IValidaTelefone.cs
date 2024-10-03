namespace API_Agenda.Services;

public interface IValidaTelefone
{
    public Task<bool> ValidaTelefoneAsync(string telefone, int contatoId);
}
