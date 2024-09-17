using API_Agenda.DTOs.Mappings;
using API_Agenda.Models;
using API_Agenda.Repository;
using API_Agenda.Services;
using APIAgenda.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace APIAgendaxUnitTests.UnitTests;

public class ContatosUnitTestController
{
    public IUnitOfwork repository;
    public IMapper mapper;

    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString =
        "Server=localhost;DataBase=Agenda;Uid=root;pwd=root";

   // Este método estático é executado apenas uma vez quando a classe é carregada,
   // Inicializando as opções do DbContext com a connection string e a versão do servidor detectada automaticamente
    static ContatosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    // Construtor da classe, que será executado toda vez que uma nova instância de ContatosUnitTestController for criada
    public ContatosUnitTestController()
    {
        // Configuração do AutoMapper, mapeando perfis de DTO para a entidade Contato
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ContatoDTOMappingProfile());
        });

        // Criação do objeto mapper a partir da configuração do AutoMapper
        mapper = config.CreateMapper();

        // Criação de uma instância do contexto de banco de dados (AppDbContext), passando as opções de configuração (dbContextOptions)
        var context = new AppDbContext(dbContextOptions);

        // Criação de uma instância do repositório (UnitOfWork), que será usada para manipular os dados no banco de dados
        repository = new UnitOfWork(context);
    }
}
