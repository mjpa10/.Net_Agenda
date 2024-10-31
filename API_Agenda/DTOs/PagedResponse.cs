namespace API_Agenda.DTOs;

public class PagedResponse<T>
{
    public IEnumerable<T>? Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}

