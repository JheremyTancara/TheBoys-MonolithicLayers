using Api.Services;
using Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;
using Api.Repositories.Interface;
using Api.Models.Interface;
using Api.Models;

namespace Api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IRepository<IMovie, MovieDTO> movieRepository;

        public MovieController(IRepository<IMovie, MovieDTO> _movieRepository)
        {
            movieRepository = _movieRepository;
        }

        [HttpGet("home-page", Name = "GetMovies")]
        public async Task<IEnumerable<MovieHomePage>> Get()
        {
            return (IEnumerable<MovieHomePage>)await movieRepository.GetAllAsync();
        }

        [HttpGet("partial-detail/{id}", Name = "GetMovie")]
        public async Task<ActionResult<MoviePartialDetail>> GetByIdPartialDetail(int id)
        {
            var movieDetail = await movieRepository.GetByIdAsync(id);

            if (movieDetail == null)
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
            var partialDetail = (MoviePartialDetail)movieDetail;
            return Ok(partialDetail);
        }

        [HttpPost(Name = "AddMovie")]
        public async Task<IActionResult> Create([FromBody] MovieDTO movieDTO)
        {
            var newMovie = await movieRepository.CreateAsync(movieDTO);
            var movie = newMovie as Movie;
            
            if (movie == null)
            {
                return BadRequest("Error creating the movie."); 
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

            var userToUpdate = await movieRepository.GetByIdAsync(id);

            if (userToUpdate != null)
            {
                await movieRepository.Update(id, MovieDTO);
                return NoContent();
            }
            else
            {
                return NotFound(ErrorUtilities.FieldNotFound("Movie", id));
            }
        }
    }
}
