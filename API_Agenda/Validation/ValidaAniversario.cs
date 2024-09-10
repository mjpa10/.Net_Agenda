
namespace API_Agenda.Services;

public class ValidaAniversario : IValidaAniversario
{
    public Task<bool> AniversarioValidoAsync(DateOnly? aniversario)
    {
        if (!aniversario.HasValue)
            return Task.FromResult(true); // como o aniversário não é obrigatório, retorna true para valores nulos

        var dataAtual = DateOnly.FromDateTime(DateTime.Now);

        if (aniversario.Value > dataAtual)
            return Task.FromResult(false); // Aniversário não pode ser no futuro

        return Task.FromResult(true);
    }
}
