namespace API_Agenda.Repository;

public interface IUnitOfwork
{
   IContatoRepository ContatoRepository { get; }

    void Commit();
}
