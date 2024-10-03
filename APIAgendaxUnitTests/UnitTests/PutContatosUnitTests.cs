using API_Agenda.Controllers;
using API_Agenda.DTOs;
using API_Agenda.Models;
using API_Agenda.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace APIAgendaxUnitTests.UnitTests;


public class PutContatosUnitTests : IClassFixture<ContatosUnitTestController>
{
    private readonly ContatosController _controller;
    private readonly Mock<IValidadorContato> _validadorContatoMock;
    public PutContatosUnitTests(ContatosUnitTestController controller)
    {
        _controller = new ContatosController(controller.repository, controller.validadorContatoMock.Object, controller.mapper);
        _validadorContatoMock = controller.validadorContatoMock;
    }

    [Fact]
    public async Task Post_Created()
    {
        // Arrange
        var contatoId = 45;

        var UpdateContatoDto = new ContatoDTO
        {
            Id = contatoId,
            Nome = "Matheus",
            Telefone = "819956608880",
            Email = "M.theus.jose.pereira@gmail.com"
        };

        // Configurar o validador para não retornar erros
        _validadorContatoMock.Setup(v => v.ValidarContatoAsync(It.IsAny<Contato>(), It.IsAny<int>()))
            .ReturnsAsync(new Dictionary<string, string>());

        //act
        var result = await _controller.PutAsync(contatoId, UpdateContatoDto);

        //Assert
        result.Should().NotBeNull(); //verifica  se resultado n é nulo
        result.Result.Should().BeOfType<OkObjectResult>();// verifica se é okobjectresult
        
    }

    [Fact]
    public async Task Post_Conflict()
    {
        // Arrange
        var contatoId = 3;

        var UpdateContatoDto = new ContatoDTO
        {
            Id = contatoId,
            Nome = "Matheus",
            Telefone = "819956608880",
            Email = "M.theus.jose.pereira@gmail.com"
        };

        // Configurar o validador para retornar erros
        _validadorContatoMock.Setup(v => v.ValidarContatoAsync(It.IsAny<Contato>(), It.IsAny<int>()))
            .ReturnsAsync(new Dictionary<string,string> {
                    { "Email", "E-mail já cadastrado." }
            });

        //act
        var result = await _controller.PutAsync(contatoId, UpdateContatoDto);

        // Assert
        var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Post_BadRequest()
    {
        // Arrange
        var contatoId = 3;

        var UpdateContatoDto = new ContatoDTO
        {
            Id = 1,
            Nome = null,
            Telefone = null,
            Email = null
        };

        //act
        var result = await _controller.PutAsync(contatoId, UpdateContatoDto);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400); // Verifica o status code 400 (BadRequest)
        badRequestResult.Value.Should().Be("Dados invalidos"); // Verifica a mensagem de erro retornada
    }
}



