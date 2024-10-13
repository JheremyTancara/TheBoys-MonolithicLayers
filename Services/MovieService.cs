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
        newMovie.Rating = ConvertToRating(newMovieDTO.Rating);
        newMovie.Title = newMovieDTO.Title;
        newMovie.Description = newMovieDTO.Description;
        newMovie.TrailerUrl = newMovieDTO.TrailerUrl;

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
        existingMovie.Rating = ConvertToRating(movieDTO.Rating);
        existingMovie.Title = movieDTO.Title;
        existingMovie.Description = movieDTO.Description;
        existingMovie.TrailerUrl = movieDTO.TrailerUrl;

        await _context.SaveChangesAsync();
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
      
      public static DateTime ConvertToDateTime(string dateString)
      {
          if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
          {
              return date; 
          }
          
          throw new FormatException("La fecha no tiene un formato válido.");
      }

      public static Genre ParseGenre(string genreString)
      {
          if (Enum.TryParse<Genre>(genreString, true, out var genre))
          {
              return genre;
          }

          throw new ArgumentException($"'{genreString}' is not a valid Genre.");
      }

      public static Rating ConvertToRating(string ratingString)
      {
          if (Enum.TryParse<Rating>(ratingString, true, out var rating))
          {
              return rating;
          }

          throw new ArgumentException($"'{ratingString}' is not a valid Rating.");
      }

      public static double ConvertToMinutes(string time)
      {
          string[] parts = time.Split(':');

          if (parts.Length != 3)
          {
              throw new FormatException("The format must be HH:MM:SS");
          }

          int hours = int.Parse(parts[0]);
          int minutes = int.Parse(parts[1]);
          int seconds = int.Parse(parts[2]);

          double totalMinutes = hours * 60 + minutes + (double)seconds / 60;
          return Math.Round(totalMinutes, 2);
      }
    }
}
