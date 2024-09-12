﻿
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;

namespace API_Agenda.Services;

public class ValidaEmail : IValidaEmail
{
    private readonly AppDbContext _context;

    public ValidaEmail(AppDbContext context) 
    {
        _context = context;
    }

    public async Task<bool> EmailJaExisteAsync(string email)
    {
        return await _context.Contatos.AnyAsync(a => a.Email == email);
    }
}