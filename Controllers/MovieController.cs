using Api.Services;
using Api.Models;
using Api.Utilities;
using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _service;

        public MovieController(MovieService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetMovies")]
        public async Task<IEnumerable<Movie>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var movie = await _service.GetByID(id);

            if (movie == null)
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
            return Ok(movie);
        }

        [HttpPost(Name = "AddMovie")]
        public async Task<IActionResult> Create([FromBody] MovieDTO movieDTO)
        {
            var newMovie = await _service.Create(movieDTO);
            if (newMovie.Title.Equals("error_409_validations"))
            {
                return Conflict(ErrorUtilities.UniqueName("Movie"));
            }

            return CreatedAtAction(nameof(GetById), new { id = newMovie.MovieID }, movieDTO);
        }

        [HttpPut("{id}", Name = "EditMovie")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDTO movieDTO)
        {
            if (id <= 0)
            {
                return BadRequest(ErrorUtilities.IdPositive(id));
            }

            var userToUpdate = await _service.GetByID(id);

            if (userToUpdate != null)
            {
                await _service.Update(id, movieDTO);
                return NoContent();
            }
            else
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
        }
    }
}
