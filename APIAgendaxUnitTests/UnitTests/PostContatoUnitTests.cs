
using API_Agenda.Controllers;
using API_Agenda.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APIAgendaxUnitTests.UnitTests;


public class PostContatoUnitTests : IClassFixture<ContatosUnitTestController>
{
    private readonly ContatosController _controller;
    public PostContatoUnitTests(ContatosUnitTestController controller)
    {
        _controller = new ContatosController(controller.repository, controller.validadorContato, controller.mapper);
    }

    [Fact]
    public async Task Post_OK()
    {
        //Arrage
        var novoContatoDto = new ContatoDTO
        {
            Nome= "Matheus",
            Telefone= "819956608880",
            Email= "M.theus.jose.pereira@gmail.com"
           
        };

        //act
        var data = await _controller.PostAsync(novoContatoDto);

        //Assert
       var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);            
    }
}
