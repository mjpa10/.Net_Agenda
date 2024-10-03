namespace API_Agenda.Services;

public interface IValidaEmail
{
    public Task<bool> EmailJaExisteAsync(string email, int contatoId);
}
