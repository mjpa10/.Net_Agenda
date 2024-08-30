using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace APIAgenda.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contato>? Contatos { get; set; }
}
