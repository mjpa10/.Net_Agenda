using APIAgenda.Context;

namespace API_Agenda.Repository
{
    public class UnitOfWork : IUnitOfwork
    {
        private AppDbContext _context;

        // Repositório de contatos, que será inicializado conforme necessário.
        private IContatoRepository? _contatoRepo;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Propriedade que retorna uma instância do repositório de contatos.
        public IContatoRepository ContatoRepository
        {
            get
            {
                // Verifica se a instância do repositório já foi criada. Se não, cria uma nova instância.
                return _contatoRepo = _contatoRepo ?? new ContatoRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
          await  _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
