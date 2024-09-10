using API_Agenda.Models;

namespace API_Agenda.Services;

public class ValidadorContato : IValidadorContato
{
    private readonly IValidaEmail _validaEmail;
    private readonly IValidaTelefone _validaTelefone;
    private readonly IValidaAniversario _validaAniversario;

    public ValidadorContato(IValidaEmail validaEmail,
                            IValidaTelefone validaTelefone,
                            IValidaAniversario validaAniversario)
    {
        _validaEmail = validaEmail;
        _validaTelefone = validaTelefone;
        _validaAniversario = validaAniversario;
    }

    public async Task<List<string>> ValidarContatoAsync(Contato contato)
    {
        var erros = new List<string>();

        if (await _validaEmail.EmailJaExisteAsync(contato.Email))
            erros.Add("E-mail já cadastrado.");

        if (await _validaTelefone.ValidaTelefoneAsync(contato.Telefone))
            erros.Add("Telefone já cadastrado.");

        if (!await _validaAniversario.AniversarioValidoAsync(contato.Aniversario))
            erros.Add("Data de aniversário está fora do intervalo permitido, no futuro.");

        return erros;
    }
}
