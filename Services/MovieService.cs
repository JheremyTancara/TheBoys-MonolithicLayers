using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Api.Services

{
    public class MovieService
    {
      private readonly DataContext _context;

      public MovieService(DataContext context)
      {
          _context = context;
      }

      public async Task<IEnumerable<MovieHomePageDTO>> GetAllHomePage()
      {
          var movies = await _context.Movies.ToListAsync();

          var homePageMovies = movies.Select(m => new MovieHomePageDTO
          {
              MovieID = m.MovieID,
              Title = m.Title,
              Genre = string.Join(", ", m.Genre.Select(g => g.ToString())), 
              Duration = $"{(int)m.Duration / 60}:{m.Duration % 60:00}", 
              Rating = m.Rating,
              ImageUrl = m.ImageUrl,
              Type = m.Type.ToString(),
              Views = m.Views,
              AgeRestriction = m.AgeRestriction 
          });

          return homePageMovies;
      }

      public async Task<MoviePartialDetailDTO?> GetByIDPartialDetail(int id)
      {
          var movie = await _context.Movies.FindAsync(id) ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");

          var partialDetailDTO = new MoviePartialDetailDTO
          {
              MovieID = movie.MovieID,
              ReleaseDate = movie.ReleaseDate.ToString("dd/MM/yyyy"), 
              Description = movie.Description,
              TrailerUrl = movie.TrailerUrl,
              CastName = movie.Cast,
              DirectorName = movie.Director
          };

          return partialDetailDTO;
      }

      public async Task<Movie?> GetByID(int id)
      {
        return await _context.Movies.FindAsync(id);
      }

      public async Task<Movie> Create(MovieDTO newMovieDTO)
      {
          await ValidateIds(newMovieDTO);

          var newMovie = new Movie
          {
              MovieID = await GetCount() + 1,
              Title = newMovieDTO.Title,
              Genre = ParseGenre(newMovieDTO.Genre),
              ReleaseDate = ConvertToDateTime(newMovieDTO.ReleaseDate),
              Duration = ConvertToMinutes(newMovieDTO.Duration),
              Rating = newMovieDTO.Rating,
              Description = newMovieDTO.Description,
              Cast = ParseCastNames(ParseListActor(newMovieDTO.CastIDs)),
              Director = ParseDirector(newMovieDTO.DirectorID).Name,
              ImageUrl = newMovieDTO.ImageUrl,
              TrailerUrl = newMovieDTO.TrailerUrl,
              Type = ConvertToContentType(newMovieDTO.Type),
              Views = newMovieDTO.Views,
              AgeRestriction = newMovieDTO.AgeRestriction,
          };

          _context.Movies.Add(newMovie);
          await _context.SaveChangesAsync();

          return newMovie;
      }

      public async Task Update(int id, MovieDTO movieDTO)
      {
        var existingMovie = await GetByID(id);

        if (existingMovie is not null)
        {

        await ValidateIds(movieDTO);

        existingMovie.Title = movieDTO.Title;
        existingMovie.Genre = ParseGenre(movieDTO.Genre);
        existingMovie.ReleaseDate = ConvertToDateTime(movieDTO.ReleaseDate);
        existingMovie.Duration = ConvertToMinutes(movieDTO.Duration);
        existingMovie.Rating = movieDTO.Rating;
        existingMovie.Title = movieDTO.Title;
        existingMovie.Description = movieDTO.Description;
        existingMovie.Cast = ParseCastNames(ParseListActor(movieDTO.CastIDs));
        existingMovie.Director = ParseDirector(movieDTO.DirectorID).Name;
        existingMovie.ImageUrl = movieDTO.ImageUrl;
        existingMovie.TrailerUrl = movieDTO.TrailerUrl;
        existingMovie.Type = ConvertToContentType(movieDTO.Type);
        existingMovie.Views = movieDTO.Views;
        existingMovie.AgeRestriction = movieDTO.AgeRestriction;

        await _context.SaveChangesAsync();
        }
      }

      public async Task UpdateViews(int id, int views)
      {
          var existingMovie = await GetByID(id);

          if (existingMovie is not null)
          {
              existingMovie.Views = views;
              await _context.SaveChangesAsync();
          }
          else
          {
              throw new KeyNotFoundException($"Movie with ID {id} not found.");
          }
      }

      public async Task<int> GetCount()
      {
        return await _context.Movies.CountAsync();
      }

      public async Task<bool> IsBrandNameUnique(string movieTitle)
      {
      var movies = await _context.Movies.AsNoTracking().ToListAsync();
      return movies.Any(b => string.Equals(b.Title, movieTitle, StringComparison.OrdinalIgnoreCase));
      }

      public async Task<Movie?> GetByTitle(string title) 
      {
          return await _context.Movies
              .FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
      }

      public async Task<IEnumerable<Movie>> GetByGenres(string genreString)
      {
          List<Genre> genres;
          try
          {
              genres = ParseGenre(genreString); 
          }
          catch (ArgumentException ex)
          {
              return Enumerable.Empty<Movie>();
          }

          var movies = await _context.Movies
              .AsNoTracking()
              .ToListAsync(); 

          return movies
              .Where(m => m.Genre.Any(g => genres.Contains(g)))
              .ToList();
      }

      public async Task<IEnumerable<Movie>> GetByContentType(string contentType)
      {
          if (Enum.TryParse<ContentType>(contentType, true, out var parsedContentType))
          {
              return await _context.Movies
                  .AsNoTracking()
                  .Where(m => m.Type == parsedContentType)
                  .ToListAsync();
          }
          
          return Enumerable.Empty<Movie>();
      }

      public static DateTime ConvertToDateTime(string dateString)
      {
          if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
          {
              return date; 
          }
          
          throw new FormatException("The date is not in a valid format.");
      }

      public static List<Genre> ParseGenre(string genreString)
      {
          if (string.IsNullOrWhiteSpace(genreString))
              throw new ArgumentException("Input string cannot be null or empty.");

          var genreStrings = genreString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .ToList();

          var genreList = new List<Genre>();

          foreach (var genre in genreStrings)
          {
              if (Enum.TryParse<Genre>(genre, true, out var parsedGenre))
              {
                  genreList.Add(parsedGenre);
              }
              else
              {
                  throw new ArgumentException($"'{genre}' is not a valid Genre.");
              }
          }

          return genreList;
      }


      public static ContentType ConvertToContentType(string contentTypeString)
      {
          if (Enum.TryParse<ContentType>(contentTypeString, true, out var contentType))
          {
              return contentType;
          }

          throw new ArgumentException($"'{contentTypeString}' is not a valid Rating.");
      }

      public static double ConvertToMinutes(string time)
      {
          string[] parts = time.Split(':');

          if (parts.Length != 2)
          {
              throw new FormatException("The format must be HH:MM");
          }

          int hours = int.Parse(parts[0]);
          int minutes = int.Parse(parts[1]);

          double totalMinutes = hours * 60 + minutes;
          return Math.Round(totalMinutes, 2);
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
