namespace API_Agenda.Models;

public class ErrorResponse
{
    public Dictionary<string, string> Errors { get; set; }

    public ErrorResponse(Dictionary<string, string> errors)
    {
        Errors = errors;
    }
}
