using API_Agenda.Controllers;
using API_Agenda.DTOs;
using API_Agenda.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAgendaxUnitTests.UnitTests;

public class DeleteContatosUnitTest : IClassFixture<ContatosUnitTestController>
{
    private readonly ContatosController _controller;
    private readonly Mock<IValidadorContato> _validadorContatoMock;
    public DeleteContatosUnitTest(ContatosUnitTestController controller)
    {
        _controller = new ContatosController(controller.repository, controller.validadorContatoMock.Object, controller.mapper);
        _validadorContatoMock = controller.validadorContatoMock;
    }

    [Fact]
    public async Task Delete_ReturnOk()
    {
        // Arrange
        var contatoId = 46;

        //act
       var result = await _controller.Delete(contatoId);

        //Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Delete_ReturnNotFound()
    {
        // Arrange
        var contatoId = 1;

        //act
        var result = await _controller.Delete(contatoId);

        //Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>()
        .Which.StatusCode.Should().Be(404);
    }


}
