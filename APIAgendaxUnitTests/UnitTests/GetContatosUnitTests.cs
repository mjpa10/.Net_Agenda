using API_Agenda.Controllers;
using API_Agenda.DTOs;
using API_Agenda.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace APIAgendaxUnitTests.UnitTests;

public class GetContatosUnitTests : IClassFixture<ContatosUnitTestController>
{
    private readonly ContatosController _controller;
    public GetContatosUnitTests(ContatosUnitTestController controller)
    {
        _controller = new ContatosController(controller.repository, controller.validadorContatoMock.Object, controller.mapper);
    }

    [Fact]
    public async Task GetAall_OKResult()
    {
        //act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
             .Which.Value.Should().BeAssignableTo<IEnumerable<ContatoDTO>>()
             .And.NotBeNull();
    }


    [Fact]
    public async Task GetId_OKResult()
    {
        //Arrange
        var id = 45; 

        //act
        var data = await _controller.GetByIdAsync(id);

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
             .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetId_NotFound()
    {
        //Arrange
        var id = 999;

        //act
        var data = await _controller.GetByIdAsync(id);

        //Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>()
             .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetId_BadRequest()
    {
        //Arrange
        var id = 0;

        //act
        var data = await _controller.GetByIdAsync(id);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>()
             .Which.StatusCode.Should().Be(400);
    }

}
