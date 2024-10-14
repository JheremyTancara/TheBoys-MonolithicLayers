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

        [HttpGet("by-title/{title}", Name = "GetMovieByTitle")]
        public async Task<ActionResult<Movie>> GetByTitle(string title)
        {
            var movie = await _service.GetByTitle(title);

            if (movie == null)
            {
                return NotFound(ErrorUtilities.valueNotFound("Movie", title));
            }

            return Ok(movie);
        }

        [HttpGet("by-genres/{genres}", Name = "GetMoviesByGenres")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetByGenres(string genres)
        {
            var movies = await _service.GetByGenres(genres);

            if (movies == null || !movies.Any())
            {
                return NotFound(ErrorUtilities.valueNotFound("Movies", genres));
            }
            
            return Ok(movies);
        }

        [HttpGet("by-content-type/{contentType}", Name = "GetMoviesByContentType")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetByContentType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return BadRequest("El tipo de contenido no puede estar vacío.");
            }

            var movies = await _service.GetByContentType(contentType);

            if (movies == null || !movies.Any())
            {
                return NotFound(ErrorUtilities.valueNotFound("Movies", contentType));
            }

            return Ok(movies);
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

        [HttpPut("{id}/views", Name = "UpdateMovieViews")]
        public async Task<IActionResult> UpdateViews(int id, [FromBody] int views)
        {
            if (id <= 0)
            {
                return BadRequest(ErrorUtilities.IdPositive(id));
            }

            try
            {
                await _service.UpdateViews(id, views);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
        }
    }
}
