using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Api.Services

{
    public class DataTransformationService
    {
      public async Task<bool> IsBrandNameUnique(DataContext _context, string userName)
      {
      var users = await _context.Users.AsNoTracking().ToListAsync();
      return users.Any(b => string.Equals(b.Username, userName, StringComparison.OrdinalIgnoreCase));
      }
    
      public static DateTime ConvertToDateTime(string dateString)
      {
          if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
          {
              return date; 
          }
          
          throw new FormatException("The date is not in a valid format.");
      }

      public static SubscriptionLevel ConvertToSubscriptionLevel(string subscriptionLevel)
      {
          if (Enum.TryParse<SubscriptionLevel>(subscriptionLevel, true, out var level))
          {
              return level;
          }
          
          throw new ArgumentException($"The value '{subscriptionLevel}' is not a valid subscription level.");
      }

      public static ProfileSkin ConvertToProfileSkin(string profileSkin)
      {
          if (Enum.TryParse<ProfileSkin>(profileSkin, true, out var skin))
          {
              return skin;
          }

          throw new ArgumentException($"The value '{profileSkin}' is not a valid profile skin.");
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