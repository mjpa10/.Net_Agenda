using API_Agenda.Models;

namespace API_Agenda.Services;

public class ValidadorContato : IValidadorContato
{
    private readonly IValidaEmail _validaEmail;
    private readonly IValidaTelefone _validaTelefone;

    public ValidadorContato(IValidaEmail validaEmail,
                            IValidaTelefone validaTelefone)
    {
        _validaEmail = validaEmail;
        _validaTelefone = validaTelefone;
    }

    public async Task<Dictionary<string, string>> ValidarContatoAsync(Contato contato, int id)
    {
        var erros = new Dictionary<string, string>();

        // Verifica se o e-mail já existe, ignorando o contato atual se o ID for o mesmo
        if (await _validaEmail.EmailJaExisteAsync(contato.Email, id))
            erros["Email"] = "E-mail já cadastrado.";

        // Verifica se o telefone já existe, ignorando o contato atual se o ID for o mesmo
        if (await _validaTelefone.ValidaTelefoneAsync(contato.Telefone, id))
            erros["Telefone"] = "Telefone já cadastrado.";

        return erros;
    }
}
