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

      public async Task<IEnumerable<Movie>> GetAll()
      {
        return await _context.Movies.ToListAsync();
      }

      public async Task<Movie?> GetByID(int id)
      {
        return await _context.Movies.FindAsync(id);
      }

      public async Task<Movie> Create(MovieDTO newMovieDTO)
      {
        var newMovie = new Movie();
        newMovie.MovieID = await GetCount() + 1;
        newMovie.Title = newMovieDTO.Title;
        newMovie.Genre = ParseGenre(newMovieDTO.Genre);
        newMovie.ReleaseDate = ConvertToDateTime(newMovieDTO.ReleaseDate);
        newMovie.Duration = ConvertToMinutes(newMovieDTO.Duration);
        newMovie.Rating = newMovieDTO.Rating;
        newMovie.Title = newMovieDTO.Title;
        newMovie.Description = newMovieDTO.Description;
        newMovie.ImageUrl = newMovieDTO.ImageUrl;
        newMovie.TrailerUrl = newMovieDTO.TrailerUrl;
        newMovie.Type = ConvertToContentType(newMovieDTO.Type);
        newMovie.Views = newMovieDTO.Views;

        _context.Movies.Add(newMovie);
        await _context.SaveChangesAsync();

        return newMovie;
      }

      public async Task Update(int id, MovieDTO movieDTO)
      {
        var existingMovie = await GetByID(id);

        if (existingMovie is not null)
        {
        existingMovie.Title = movieDTO.Title;
        existingMovie.Genre = ParseGenre(movieDTO.Genre);
        existingMovie.ReleaseDate = ConvertToDateTime(movieDTO.ReleaseDate);
        existingMovie.Duration = ConvertToMinutes(movieDTO.Duration);
        existingMovie.Rating = movieDTO.Rating;
        existingMovie.Title = movieDTO.Title;
        existingMovie.Description = movieDTO.Description;
        existingMovie.ImageUrl = movieDTO.ImageUrl;
        existingMovie.TrailerUrl = movieDTO.TrailerUrl;
        existingMovie.Type = ConvertToContentType(movieDTO.Type);
        existingMovie.Views = movieDTO.Views;

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

      public async Task Delete(int id)
      {
        var movieToDelete = await GetByID(id);

        if(movieToDelete is not null)
        {
          _context.Movies.Remove(movieToDelete);
          await _context.SaveChangesAsync();
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

    }
}
