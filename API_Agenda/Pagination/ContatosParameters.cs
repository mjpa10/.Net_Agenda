namespace API_Agenda.Pagination;

public class ContatosParameters
{
    const int maxPageSize = 50;

    /// <summary>
    /// O número da página para paginação.
    /// </summary>
    public int PageNumber { get; set; } = 1;
    private int _pageSize = maxPageSize;

    /// <summary>
    /// O tamanho da página para paginação(quantidade de dados que irá aparecer).
    /// </summary>
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {   
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
