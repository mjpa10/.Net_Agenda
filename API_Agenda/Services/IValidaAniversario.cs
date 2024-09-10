namespace API_Agenda.Services
{
    public interface IValidaAniversario
    {
        public Task<bool> AniversarioValidoAsync(DateOnly? aniversario);
    }
}
