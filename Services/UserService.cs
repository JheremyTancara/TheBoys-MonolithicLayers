using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Api.Services
{
    public class UserService
    {
      private readonly DataContext _context;

      public UserService(DataContext context)
      {
          _context = context;
      }

      public async Task<IEnumerable<User>> GetAll()
      {
        return await _context.Users.ToListAsync();
      }

      public async Task<User?> GetByID(int id)
      {
        return await _context.Users.FindAsync(id);
      }

      public async Task<User> Create(UserDTO newUserDTO)
      {
        if (await IsBrandNameUnique(newUserDTO.Username))
        {
          var user = new User();
          user.Username = "error_409_validations";
          return user;
        }

        var newUser = new User();
        newUser.UserID = await GetCount() + 1;
        newUser.Username = newUserDTO.Username;
        newUser.Email = newUserDTO.Email;
        newUser.Password = newUserDTO.Password;
        newUser.DateOfBirth = ConvertToDateTime(newUserDTO.DateOfBirth);
        newUser.SubscriptionLevel = ConvertToSubscriptionLevel(newUserDTO.SubscriptionLevel);
        newUser.ProfilePicture = ConvertToProfileSkin(newUserDTO.ProfilePicture);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
      }

      public async Task Update(int id, UserDTO userDTO)
      {
        var existingUser = await GetByID(id);

        if (existingUser is not null)
        {
        existingUser.Username = userDTO.Username;
        existingUser.Email = userDTO.Email;
        existingUser.Password = userDTO.Password;
        existingUser.DateOfBirth = ConvertToDateTime(userDTO.DateOfBirth);
        existingUser.SubscriptionLevel = ConvertToSubscriptionLevel(userDTO.SubscriptionLevel);
        existingUser.ProfilePicture = ConvertToProfileSkin(userDTO.ProfilePicture);
        await _context.SaveChangesAsync();
      }
      }

      public async Task Delete(int id)
      {
        var userToDelete = await GetByID(id);

        if(userToDelete is not null)
        {
          _context.Users.Remove(userToDelete);
          await _context.SaveChangesAsync();
        }
      }

      public async Task<int> GetCount()
      {
        return await _context.Users.CountAsync();
      }

      public async Task<bool> IsBrandNameUnique(string productName)
      {
      var products = await _context.Users.AsNoTracking().ToListAsync();
      return products.Any(b => string.Equals(b.Username, productName, StringComparison.OrdinalIgnoreCase));
      }
      
      public static DateTime ConvertToDateTime(string dateString)
      {
          if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
          {
              return date; 
          }
          
          throw new FormatException("La fecha no tiene un formato válido.");
      }

      public static SubscriptionLevel ConvertToSubscriptionLevel(string subscriptionLevel)
      {
          if (Enum.TryParse<SubscriptionLevel>(subscriptionLevel, true, out var level))
          {
              return level;
          }
          
          throw new ArgumentException($"El valor '{subscriptionLevel}' no es un nivel de suscripción válido.");
      }

      public static ProfileSkin ConvertToProfileSkin(string profileSkin)
      {
          if (Enum.TryParse<ProfileSkin>(profileSkin, true, out var skin))
          {
              return skin;
          }

          throw new ArgumentException($"El valor '{profileSkin}' no es un perfil de skin válido.");
      }
      }
}
