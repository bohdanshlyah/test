using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopFront.Application.User.Commands;
using ShopFront.Application.User.Queries;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.WebApi.Helpers;
using System.Security.Claims;


namespace ShopFront.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("sign-up")]
    public async Task<ActionResult<Guid>> SignUp([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(command, cancellationToken);

        var accessToken = JwtTokenHelper.GenerateAccessToken(user);
        var refreshToken = JwtTokenHelper.GenerateRefreshToken();

        await _mediator.Send(new UpdateRefreshTokenCommand
        {
            Id = user.Id,
            RefreshToken = refreshToken
        }, cancellationToken);

        JwtTokenHelper.SetAccessToken(Response, new AccessToken
        {
            Token = accessToken,
            ExpiryTime = refreshToken.ExpiryTime
        });
        JwtTokenHelper.SetRefreshToken(Response, refreshToken);

        return Ok(user.Id);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("sign-in")]
    public async Task<ActionResult<Guid>> SignIn([FromBody] VerifyUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(query, cancellationToken);

        var accessToken = JwtTokenHelper.GenerateAccessToken(user);
        var refreshToken = JwtTokenHelper.GenerateRefreshToken();

        await _mediator.Send(new UpdateRefreshTokenCommand
        {
            Id = user.Id,
            RefreshToken = refreshToken
        }, cancellationToken);

        JwtTokenHelper.SetAccessToken(Response, new AccessToken
        {
            Token = accessToken,
            ExpiryTime = refreshToken.ExpiryTime
        });
        JwtTokenHelper.SetRefreshToken(Response, refreshToken);

        return Ok(user.Id);
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("sign-out"), Authorize]
    public async Task<ActionResult> SignOut(DeleteUserRefreshTokenQuery query, CancellationToken cancellationToken)
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        await _mediator.Send(query, cancellationToken);

        return NoContent();
    }

    [ProducesResponseType(typeof(ICollection<UserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet, Authorize]
    public async Task<ActionResult<ICollection<UserViewModel>>> GetUsers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);

        return Ok(result);
    }

    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:guid}"), Authorize]
    public async Task<ActionResult<UserViewModel>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery
        {
            Id = id
        }, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{email}"), Authorize]
    public async Task<ActionResult<UserViewModel>> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByEmailQuery
        {
            Email = email
        }, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("update"), Authorize]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        Guid userId = new Guid();
        Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
        if (userId != command.Id)
        {
            return BadRequest("The user can only update their own profile");
        }
        var result = await _mediator.Send(command, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("delete/{id}"), Authorize]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand
        {
            Id = id
        }, cancellationToken);

        return NoContent();
    }
}