
using API_Agenda.Models;
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;

namespace API_Agenda.Services;

public class ValidaTelefone : IValidaTelefone
{
    private readonly AppDbContext _context;

    public ValidaTelefone(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ValidaTelefoneAsync(string telefone, int contatoId)
    {
        return await _context.Contatos.AnyAsync(a => a.Telefone == telefone && a.Id != contatoId);
    }
}
