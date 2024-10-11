using Api.Services;
using Api.Models;
using Api.Utilities;
using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ActorService _service;

        public ActorController(ActorService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetActors")]
        public async Task<IEnumerable<Actor>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}", Name = "GetActor")]
        public async Task<ActionResult<Actor>> GetById(int id)
        {
            var actor = await _service.GetByID(id);

            if (actor == null)
            {
                return NotFound(ErrorUtilities.FieldNotFound("Actor", id));
            }
            return Ok(actor);
        }

        [HttpPost(Name = "AddActor")]
        public async Task<IActionResult> Create([FromBody] ActorDTO actorDTO)
        {
            var newActor = await _service.Create(actorDTO);
            if (newActor.Name.Equals("error_409_validations"))
            {
                return Conflict(ErrorUtilities.UniqueName("Actor"));
            }

            return CreatedAtAction(nameof(GetById), new { id = newActor.ActorID }, actorDTO);
        }

        [HttpPut("{id}", Name = "EditActor")]
        public async Task<IActionResult> Update(int id, [FromBody] ActorDTO actorDTO)
        {
            if (id <= 0)
            {
                return BadRequest(ErrorUtilities.IdPositive(id));
            }

            var actorToUpdate = await _service.GetByID(id);

            if (actorToUpdate != null)
            {
                await _service.Update(id, actorDTO);
                return NoContent();
            }
            else
            {
                return NotFound(ErrorUtilities.FieldNotFound("Actor", id));
            }
        }
    }
}
