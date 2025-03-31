using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MediatR;
using InTouch.UserService.Extensions;
using InTouch.UserService.Models;
using InTouch.UserService.Application;

namespace InTouch.UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<CreatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody][Required] CreatePermissionCommand command) =>
        (await mediator.Send(command)).ToActionResult();
}