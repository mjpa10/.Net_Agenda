using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Agenda.Models;

[Table("Contatos")]
public class Contato {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "O número de telefone informado não é válido.")]
    [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    public string? Telefone { get; set; }
}
