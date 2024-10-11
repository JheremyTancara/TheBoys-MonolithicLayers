using Api.Services;
using Api.Models;
using Api.Utilities;
using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<User>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _service.GetByID(id);

            if (user == null)
            {
                return NotFound(ErrorUtilities.FieldNotFound("User", id));
            }
            return Ok(user);
        }

        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            var newUser = await _service.Create(userDTO);
            if (newUser.Username.Equals("error_409_validations"))
            {
                return Conflict(ErrorUtilities.UniqueName("User"));
            }

            return CreatedAtAction(nameof(GetById), new { id = newUser.UserID }, userDTO);
        }

        [HttpPut("{id}", Name = "EditUser")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDTO)
        {
            if (id <= 0)
            {
                return BadRequest(ErrorUtilities.IdPositive(id));
            }

            var userToUpdate = await _service.GetByID(id);

            if (userToUpdate != null)
            {
                await _service.Update(id, userDTO);
                return NoContent();
            }
            else
            {
                return NotFound(ErrorUtilities.FieldNotFound("User", id));
            }
        }
    }
}
