using Api.Services;
using Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;

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

        [HttpGet("home-page", Name = "GetMovies")]
        public async Task<IEnumerable<MovieHomePageDTO>> Get()
        {
            return await _service.GetAllHomePage();
        }

        [HttpGet("partial-detail/{id}", Name = "GetMovie")]
        public async Task<ActionResult<MoviePartialDetailDTO>> GetByIdPartialDetail(int id)
        {
            var movieDetail = await _service.GetByIDPartialDetail(id);

            if (movieDetail == null)
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }

            return Ok(movieDetail);
        }

        [HttpPost(Name = "AddMovie")]
        public async Task<IActionResult> Create([FromBody] MovieDTO movieDTO)
        {
            var newMovie = await _service.Create(movieDTO);
            if (newMovie.Title.Equals("error_409_validations"))
            {
                return Conflict(ErrorUtilities.UniqueName("Movie"));
            }

            return CreatedAtAction(nameof(GetByIdPartialDetail), new { id = newMovie.MovieID }, movieDTO);
        }

        [HttpPut("{id}", Name = "EditMovie")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDTO MovieDTO)
        {
            if (id <= 0)
            {
                return BadRequest(ErrorUtilities.IdPositive(id));
            }

            var userToUpdate = await _service.GetByID(id);

            if (userToUpdate != null)
            {
                await _service.Update(id, MovieDTO);
                return NoContent();
            }
            else
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
        }
    }
}
