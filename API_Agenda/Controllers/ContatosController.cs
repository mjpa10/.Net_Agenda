using API_Agenda.Models;
using API_Agenda.Repository;
using API_Agenda.Services;
using APIAgenda.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Agenda.Controllers;

[ApiController]
[Route("[controller]")]
public class ContatosController : ControllerBase
{
    private readonly IValidadorContato _validadorContato;
    private readonly IContatoRepository _repository;

    public ContatosController(IContatoRepository repository,
                               IValidadorContato validadorContato)
    {
        _validadorContato = validadorContato;
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contato>>> Get()
    {
       var contatos = await _repository.GetAllAsync();

       return Ok(contatos);
    }

    [HttpGet("{id:int}", Name="ObterContato")]
    public async Task<ActionResult<Contato>> GetByIdAsync(int id)
    {
        var contato = await _repository.GetContatoAsync(id);
        
        if (contato == null) 
            return NotFound("Contato não encontrado...");

        return Ok(contato);
    }

    [HttpPost]
    public async Task<ActionResult<Contato>> PostAsync([FromBody]Contato contato)
    {

        if (contato == null)
            return BadRequest("Dados Inválidos");

        var erros = await _validadorContato.ValidarContatoAsync(contato); //um contador de erros para que todos sejam exibidos

        if (erros.Any())
            return Conflict(string.Join("\n", erros));//exibe todos os erros da criacao, separados por uma quebra de linha

        var contatoCriado = _repository.Create(contato);

        return new CreatedAtRouteResult("ObterContato",
            new { id = contatoCriado.Id }, contatoCriado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Contato>> PutAsync(int id,[FromBody] Contato contato)
    {
        if (id != contato.Id)
            return BadRequest("Identidicadores diferentes");

       _repository.Update(contato);        

        return Ok(contato);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Contato>> Delete(int id)
    {
        var contato = await _repository.GetContatoAsync(id);

        if (contato is null)
            return NotFound("contato não encontrado...");

        var contatoExcluido = _repository.Delete(id);

        return Ok(contatoExcluido);
    }
}
