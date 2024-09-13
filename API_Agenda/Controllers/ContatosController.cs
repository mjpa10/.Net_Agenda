using API_Agenda.DTOs;
using API_Agenda.Models;
using API_Agenda.Pagination;
using API_Agenda.Repository;
using API_Agenda.Services;
using APIAgenda.Context;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_Agenda.Controllers;

[ApiController]
[Route("[controller]")]
public class ContatosController : ControllerBase
{
    private readonly IValidadorContato _validadorContato;
    private readonly IUnitOfwork _uof;
    private readonly IMapper _mapper;

    public ContatosController(IUnitOfwork uof,
                               IValidadorContato validadorContato,
                               IMapper mapper)
    {
        _validadorContato = validadorContato;
        _mapper = mapper;
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContatoDTO>>> Get()
    {        
       var contatos =  _uof.ContatoRepository.GetAll();

       var contatosDto = _mapper.Map<IEnumerable<ContatoDTO>>(contatos);

       return Ok(contatosDto);
    }


    [HttpGet("{id:int}", Name="ObterContato")]
    public async Task<ActionResult<ContatoDTO>> GetByIdAsync(int id)
    {        
        var contato = await _uof.ContatoRepository.GetContatoAsync(id);        
        if (contato == null)
            return NotFound($"contato com id={id} não encontrado...");

        var contatoDto = _mapper.Map<ContatoDTO>(contato); 

        return Ok(contatoDto);
    }

    [HttpPost]
    public async Task<ActionResult<ContatoDTO>> PostAsync([FromBody] ContatoDTO contatoDTO)
    {
        if (contatoDTO == null)
            return BadRequest("Dados Inválidos");

        var contato = _mapper.Map<Contato>(contatoDTO);

        var erros = await _validadorContato.ValidarContatoAsync(contato); //um contador de erros para que todos sejam exibidos

        if (erros.Any())
            return Conflict(string.Join("\n", erros));//exibe todos os erros da criacao, separados por uma quebra de linha

        var contatoCriado = _uof.ContatoRepository.Create(contato);
        _uof.Commit();

        var contatoCriadoDto = _mapper.Map<ContatoDTO>(contatoCriado);

        return new CreatedAtRouteResult("ObterContato",
            new { id = contatoCriadoDto.Id }, contatoCriadoDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ContatoDTO>> PutAsync(int id, ContatoDTO contatoDTO)
    {
        if (id != contatoDTO.Id)
            return BadRequest("Dados invalidos");

        var contato = _mapper.Map<Contato>(contatoDTO);

        _uof.ContatoRepository.Update(contato);
         _uof.Commit();

        var produtoAtualizadoDto = _mapper.Map<ContatoDTO>(contato);

        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ContatoDTO>> Delete(int id)
    {
        var contato = await _uof.ContatoRepository.GetContatoAsync(id);

        if (contato is null)
            return NotFound($"contato com id={id} não encontrado...");

        var contatoExcluido = _uof.ContatoRepository.Delete(id);
        _uof.Commit();

        var contatoExcluidoDto = _mapper.Map<ContatoDTO>(contatoExcluido);

        return Ok(contatoExcluidoDto);
    }

    //pagination
    [HttpGet("pagiation")]
    public ActionResult<IEnumerable<ContatoDTO>> Get([FromQuery]
                                ContatosParameters contatosParameters)
    {
        var contatos =  _uof.ContatoRepository.GetContatos(contatosParameters);

        var contatosDto = _mapper.Map<IEnumerable<ContatoDTO>>(contatos);
        return Ok(contatos);
    }
}
