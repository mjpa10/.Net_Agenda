using API_Agenda.DTOs;
using API_Agenda.Models;
using API_Agenda.Pagination;
using API_Agenda.Repository;
using API_Agenda.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace API_Agenda.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Obtem uma lista de contatos
    /// </summary>
    /// <returns>Uma lista de objetos ContatoDTO.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ContatoDTO>>> Get()
    {       
            var contatos = await _uof.ContatoRepository.GetAllAsync();

            if (contatos is null)
                return NotFound("Não existem contatos...");

            var contatosDto = _mapper.Map<IEnumerable<ContatoDTO>>(contatos);

            return Ok(contatosDto);
              
    }

    /// <summary>
    /// Obtém um contato pelo ID.
    /// </summary>
    /// <param name="id">ID do contato a ser buscado.</param>
    /// <returns>O contato correspondente ao ID fornecido.</returns>
    [HttpGet("{id:int}", Name = "ObterContato")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ContatoDTO>> GetByIdAsync(int id)
    {
        if (id == null || id <=0)
        {  
            return BadRequest("Id de Contato inválido");
        }

        var contato = await _uof.ContatoRepository.GetContatoAsync(id);
        if (contato == null)
            return NotFound($"contato com id={id} não encontrado...");

        var contatoDto = _mapper.Map<ContatoDTO>(contato);

        return Ok(contatoDto);
    }

    /// <summary>
    /// Cria um novo contato.
    /// </summary>
    /// <remarks>
    /// Exemplo de request
    ///         
    ///         POST api/contatos
    ///          {
    ///             "nome": "nome1",
    ///              "email": "user@example.com",
    ///               "telefone": "0000000"
    ///           }          
    /// </remarks>
    /// <param name="contatoDTO">Objeto ContatoDTO contendo os dados do novo contato.</param>
    /// <returns>O contato criado com seu ID gerado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ContatoDTO>> PostAsync([FromBody] ContatoDTO contatoDTO)
    {
        if (contatoDTO == null)
            return BadRequest("Dados Inválidos");

        var contato = _mapper.Map<Contato>(contatoDTO);

        var erros = await _validadorContato.ValidarContatoAsync(contato, 0);  // O ID será 0, pois é um novo contato

        if (erros.Any())
        {
            return Conflict(new ErrorResponse(erros)); // Retorna o dicionário de erros diretamente
        }

        var contatoCriado = _uof.ContatoRepository.Create(contato);
        await _uof.CommitAsync();

        var contatoCriadoDto = _mapper.Map<ContatoDTO>(contatoCriado);

        return new CreatedAtRouteResult("ObterContato",
            new { id = contatoCriadoDto.Id }, contatoCriadoDto);
    }

    /// <summary>
    /// Atualiza um contato existente.
    /// </summary>
    /// <param name="id">ID do contato a ser atualizado.</param>
    /// <param name="contatoDTO">Objeto ContatoDTO com os dados atualizados.</param>
    /// <returns>O contato atualizado.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ContatoDTO>> PutAsync(int id, ContatoDTO contatoDTO)
    {
        if (id != contatoDTO.Id)
            return BadRequest("Dados invalidos");

        var contato = _mapper.Map<Contato>(contatoDTO);

        var erros = await _validadorContato.ValidarContatoAsync(contato, id);

        if (erros.Any())
        {
            return Conflict(new ErrorResponse(erros)); // Retorna o dicionário de erros diretamente
        }

        _uof.ContatoRepository.Update(contato);
        await _uof.CommitAsync();

        var produtoAtualizadoDto = _mapper.Map<ContatoDTO>(contato);

        return Ok(produtoAtualizadoDto);
    }

    /// <summary>
    /// Exclui um contato pelo ID.
    /// </summary>
    /// <param name="id">ID do contato a ser excluído.</param>
    /// <returns>O contato excluído.</returns>v
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ContatoDTO>> Delete(int id)
    {
        var contato = await _uof.ContatoRepository.GetContatoAsync(id);

        if (contato is null)
            return NotFound($"contato com id={id} não encontrado...");

        var contatoExcluido = _uof.ContatoRepository.Delete(id);
        await _uof.CommitAsync();

        var contatoExcluidoDto = _mapper.Map<ContatoDTO>(contatoExcluido);

        return Ok(contatoExcluidoDto);
    }

    /// <summary>
    /// Filtra e pagina os contatos.
    /// </summary>
    /// <param name="contatosParameters">Parâmetros de paginação (número da página, tamanho da página).</param>
    /// <param name="searchTerm">Parâmetro opcional para filtrar por nome, e-mail ou telefone.</param>
    /// <returns>Uma lista paginada de objetos ContatoDTO com os filtros aplicados.</returns>
    //pagination
    [HttpGet("Filtrar")]
    public async Task<ActionResult<IEnumerable<ContatoDTO>>> GetFiltradoAsync([FromQuery] ContatosParameters contatosParameters,  // Parâmetros de paginação (número da página, tamanho da página).
                                                                              [FromQuery] string? searchTerm = null) // Parâmetro opcional para o termo de busca.
    {    
        // Obtém os contatos do repositório aplicando os parâmetros de paginação e o termo de busca (se houver).
        var contatos = await  _uof.ContatoRepository.GetContatosAsync(contatosParameters, searchTerm);

        return ObterContatos(contatos);  
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private ActionResult<IEnumerable<ContatoDTO>> ObterContatos(IPagedList<Contato> contatos)
    {
        // Cria um objeto contendo os metadados de paginação (número total de itens, tamanho da página, etc.).
        var metadata = new
        {
            contatos.Count,
            contatos.PageSize,
            contatos.PageCount,
            contatos.TotalItemCount,
            contatos.HasNextPage,
            contatos.HasPreviousPage,
        };
        // Adiciona os metadados de paginação no cabeçalho da resposta.
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var contatosDto = _mapper.Map<IEnumerable<ContatoDTO>>(contatos);

        return Ok(contatosDto);

    }
}
