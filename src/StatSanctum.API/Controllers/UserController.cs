using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StatSanctum.API.Models;
using StatSanctum.API.Queries.Users;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Models;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetAllQuery<User>());

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetUsersById")]
        public async Task<ActionResult> GetUsersById(int id)
        {
            try
            {
                var user = await _mediator.Send(new GetByIdQuery<User> { Id = id });

                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }
            try
            {
                var savedUser = await _mediator.Send(new CreateUserCommand<User> { Entity = _mapper.Map<User>(user) });
                var userDto = _mapper.Map<UserResponseDto>(savedUser);

                return CreatedAtAction(nameof(GetUsersById), new { id = userDto.UserID }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateUserById")]
        public async Task<ActionResult<User>> UpdateUserById(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }
            try
            {
                // Get existing user
                var existingUser = await _mediator.Send(new GetByIdQuery<User> { Id = id });

                var updatedUser = _mapper.Map(userDto, existingUser);

                var user = await _mediator.Send(new UpdateCommand<User> { Entity = updatedUser });

                return Ok(user); // 200 OK
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}", Name = "DeleteUserById")]
        public async Task<ActionResult> DeleteUserById(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCommand<User> { Id = id });

                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
