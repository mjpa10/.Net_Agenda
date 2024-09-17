namespace API_Agenda.Repository;

public interface IUnitOfwork
{
   IContatoRepository ContatoRepository { get; }

    Task CommitAsync();
}
