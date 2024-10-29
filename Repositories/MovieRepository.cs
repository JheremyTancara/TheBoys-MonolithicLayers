using Api.Data;
using Api.DTOs;
using Api.Models;
using Api.Models.Interface;
using Api.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Api.Services

{
    public class MovieRepository : RepositoryBase<IMovie, MovieDTO>
    {
      private readonly DataContext _context;
      public DataTransformationService genericService;

      public MovieRepository(DataContext context)
      {
          _context = context;
          genericService = new DataTransformationService();
      }

      public override async Task<IEnumerable<IMovie>> GetAllAsync()
      {
          var movies = await _context.Movies.ToListAsync();

          var homePageMovies = movies.Select(m => new MovieHomePage
          {
              MovieID = m.MovieID,
              Title = m.Title,
              Genre = string.Join(", ", m.Genre?.Select(g => g.ToString()) ?? Enumerable.Empty<string>()),
              Duration = $"{(int)m.Duration / 60}:{m.Duration % 60:00}",
              Rating = m.Rating,
              ImageUrl = m.ImageUrl,
              Type = m.Type.ToString(),
              Views = m.Views,
              AgeRestriction = m.AgeRestriction
          }).ToList();

          return homePageMovies;
      }

      public override async Task<IMovie?> GetByIdAsync(int id)
      {
          var movie = await _context.Movies.FindAsync(id) 
                      ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

          var partialDetail = new MoviePartialDetail
          {
              MovieID = movie.MovieID,
              ReleaseDate = movie.ReleaseDate.ToString("dd/MM/yyyy"),
              Description = movie.Description,
              TrailerUrl = movie.TrailerUrl,
              CastName = movie.Cast,
              DirectorName = movie.Director
          };

          return partialDetail;
      }

      public override async Task<IMovie> CreateAsync(MovieDTO newMovieDTO)
      {
          await ValidateIds(newMovieDTO);

          var newMovie = new Movie
          {
              MovieID = await _context.Movies.CountAsync() + 1,
              Title = newMovieDTO.Title,
              Genre = DataTransformationService.ParseGenre(newMovieDTO.Genre),
              ReleaseDate = DataTransformationService.ConvertToDateTime(newMovieDTO.ReleaseDate),
              Duration = DataTransformationService.ConvertToMinutes(newMovieDTO.Duration),
              Rating = newMovieDTO.Rating,
              Description = newMovieDTO.Description,
              Cast = ParseCastNames(ParseListActor(newMovieDTO.CastIDs)),
              Director = ParseDirector(newMovieDTO.DirectorID).Name,
              ImageUrl = newMovieDTO.ImageUrl,
              TrailerUrl = newMovieDTO.TrailerUrl,
              Type = DataTransformationService.ConvertToContentType(newMovieDTO.Type),
              Views = newMovieDTO.Views,
              AgeRestriction = newMovieDTO.AgeRestriction,
          };

          _context.Movies.Add(newMovie);
          await _context.SaveChangesAsync();

          return newMovie;
      }

      public override async Task Update(int id, MovieDTO movieDTO)
      {
        var existingMovie = await GetByID(id);

        if (existingMovie is not null)
        {

        await ValidateIds(movieDTO);

        existingMovie.Title = movieDTO.Title;
        existingMovie.Genre = DataTransformationService.ParseGenre(movieDTO.Genre);
        existingMovie.ReleaseDate = DataTransformationService.ConvertToDateTime(movieDTO.ReleaseDate);
        existingMovie.Duration = DataTransformationService.ConvertToMinutes(movieDTO.Duration);
        existingMovie.Rating = movieDTO.Rating;
        existingMovie.Title = movieDTO.Title;
        existingMovie.Description = movieDTO.Description;
        existingMovie.Cast = ParseCastNames(ParseListActor(movieDTO.CastIDs));
        existingMovie.Director = ParseDirector(movieDTO.DirectorID).Name;
        existingMovie.ImageUrl = movieDTO.ImageUrl;
        existingMovie.TrailerUrl = movieDTO.TrailerUrl;
        existingMovie.Type = DataTransformationService.ConvertToContentType(movieDTO.Type);
        existingMovie.Views = movieDTO.Views;
        existingMovie.AgeRestriction = movieDTO.AgeRestriction;

        await _context.SaveChangesAsync();
        }
      }

      public async Task<Movie?> GetByID(int id)
      {
        return await _context.Movies.FindAsync(id);
      }


      private List<Actor> ParseListActor(string castIdsString)
      {
          var castIdsList = castIdsString
              .Split(',')
              .Select(id => id.Trim())
              .Where(id => int.TryParse(id, out _))
              .Select(int.Parse)
              .ToList();

          var existingActors = _context.Actors
              .Where(actor => castIdsList.Contains(actor.ActorID))
              .ToList();

          return existingActors;
      }

      private Director ParseDirector(string directorIdString)
      {
          var directorId = int.Parse(directorIdString); 

          var existingDirector = _context.Directors
              .FirstOrDefault(d => d.DirectorID == directorId);

          if (existingDirector == null) throw new ArgumentException("The specified Director ID does not exist.");

          return existingDirector;
      } 

      private async Task ValidateIds(MovieDTO newMovieDTO)
      {
          var castIdsString = newMovieDTO.CastIDs; 
          var castIDsList = castIdsString
              .Split(',')
              .Select(id => id.Trim()) 
              .Where(id => int.TryParse(id, out _)) 
              .Select(int.Parse) 
              .ToList();

          var existingCastIDs = await _context.Actors
              .Where(c => castIDsList.Contains(c.ActorID))
              .Select(c => c.ActorID)
              .ToListAsync();

          if (existingCastIDs.Count != castIDsList.Count)
          {
              throw new ArgumentException("One or more Cast IDs do not exist.");
          }

          var directorId = int.Parse(newMovieDTO.DirectorID);
          var existingDirectorID = await _context.Directors
              .Where(d => d.DirectorID == directorId)
              .Select(d => d.DirectorID)
              .FirstOrDefaultAsync();

          if (existingDirectorID == 0)
          {
              throw new ArgumentException("The specified Director ID does not exist.");
          }
      }

      private string[] ParseCastNames(List<Actor>? cast)
      {
          if (cast == null)
          {
              return Array.Empty<string>(); 
          }

          return cast.Select(actor => actor.Name).ToArray();
      }
    }
}
