using API_Agenda.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAgenda.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contato>? Contatos { get; set; }
}
