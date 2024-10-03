
using API_Agenda.Controllers;
using API_Agenda.DTOs;
using API_Agenda.Models;
using API_Agenda.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace APIAgendaxUnitTests.UnitTests;


public class PostContatosUnitTests : IClassFixture<ContatosUnitTestController>
{
    private readonly ContatosController _controller;
    private readonly Mock<IValidadorContato> _validadorContatoMock;
    public PostContatosUnitTests(ContatosUnitTestController controller)
    {
        _controller = new ContatosController(controller.repository, controller.validadorContatoMock.Object, controller.mapper);
        _validadorContatoMock = controller.validadorContatoMock;
    }

    [Fact]
    public async Task Post_OK()
    {
        // Arrange
        var novoContatoDto = new ContatoDTO
        {
            Nome = "Matheus",
            Telefone = "819956608880",
            Email = "M.theus.jose.pereira@gmail.com"
        };

        // Configurar o validador para não retornar erros
        _validadorContatoMock.Setup(v => v.ValidarContatoAsync(It.IsAny<Contato>(), It.IsAny<int>()))
               .ReturnsAsync(new Dictionary<string, string>()); // Retornar dicionário vazio (sem erros)

        //act
        var data = await _controller.PostAsync(novoContatoDto);

        //Assert
       var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);            
    }

    [Fact]
    public async Task Post_Conflict_WhenValidationFails()
    {
        // Arrange
        var novoContatoDto = new ContatoDTO
        {
            Nome = "Matheus",
            Telefone = "819956608880",
            Email = "M.theus.jose.pereira@gmail.com"
        };

        // Configurar o validador para retornar erros de validação
        _validadorContatoMock.Setup(v => v.ValidarContatoAsync(It.IsAny<Contato>(), It.IsAny<int>()))
            .ReturnsAsync(new Dictionary<string, string>
            {
                    { "Email", "E-mail já cadastrado." }
            });

        // Act
        var result = await _controller.PostAsync(novoContatoDto);

        // Assert
        var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(409);

        // Verificar o conteúdo do conflito (dicionário de erros)
        var errorResponse = conflictResult.Value as ErrorResponse;
        errorResponse.Errors.Should().ContainKey("Email");
        errorResponse.Errors["Email"].Should().Be("E-mail já cadastrado.");
    }

    [Fact]
    public async Task Post_BadRequest_NullDto()
    {
        // Arrange
        ContatoDTO novoContatoDto = null; // Simulando uma requisição inválida com null

        // Act
        var result = await _controller.PostAsync(novoContatoDto);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400); // Verifica o status code 400 (BadRequest)
        badRequestResult.Value.Should().Be("Dados Inválidos"); // Verifica a mensagem de erro retornada
    }
}



