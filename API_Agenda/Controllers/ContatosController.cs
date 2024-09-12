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
    private readonly IUnitOfwork _uof;
    private readonly ILogger _logger;

    public ContatosController(IUnitOfwork uof,
                               IValidadorContato validadorContato,
                               ILogger<ContatosController> logger)
    {
        _validadorContato = validadorContato;
        _logger = logger;
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contato>>> Get()
    {
        _logger.LogInformation("=====Get api/Contatos ========");

       var contatos = await _uof.ContatoRepository.GetAllAsync();

       return Ok(contatos);
    }

    [HttpGet("{id:int}", Name="ObterContato")]
    public async Task<ActionResult<Contato>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"=====Get api/Contatos/id = {id} ========");

        var contato = await _uof.ContatoRepository.GetContatoAsync(id);
        
        if (contato == null)
            return NotFound($"contato com id={id} não encontrado...");

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

        var contatoCriado = _uof.ContatoRepository.Create(contato);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterContato",
            new { id = contatoCriado.Id }, contatoCriado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Contato>> PutAsync(int id,Contato contato)
    {

        if (id != contato.Id)
            return BadRequest("Dados invalidos");

        _uof.ContatoRepository.Update(contato);
         _uof.Commit();

        return Ok(contato);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Contato>> Delete(int id)
    {
        var contato = await _uof.ContatoRepository.GetContatoAsync(id);

        if (contato is null)
            return NotFound($"contato com id={id} não encontrado...");

        var contatoExcluido = _uof.ContatoRepository.Delete(id);
        _uof.Commit();

        return Ok(contatoExcluido);
    }
}
