using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services

{
    public class DirectorService
    {
      private readonly DataContext _context;

      public DirectorService(DataContext context)
      {
          _context = context;
      }

      public async Task<IEnumerable<Director>> GetAll()
      {
        return await _context.Directors.ToListAsync();
      }

      public async Task<Director?> GetByID(int id)
      {
        return await _context.Directors.FindAsync(id);
      }

      public async Task<Director> Create(DirectorDTO newDirectorDTO)
      {
        var newDirector = new Director();
        newDirector.DirectorID = await GetCount() + 1;
        newDirector.Name = newDirectorDTO.Name;
        newDirector.Age = newDirectorDTO.Age;
        newDirector.Bio = newDirectorDTO.Bio;
        newDirector.ProfilePictureUrl = newDirectorDTO.ProfilePictureUrl;
        newDirector.NumberOfAwards = newDirectorDTO.NumberOfAwards;

        _context.Directors.Add(newDirector);
        await _context.SaveChangesAsync();

        return newDirector;
      }

      public async Task Update(int id, DirectorDTO directorDTO)
      {
        var existingDirector = await GetByID(id);

        if (existingDirector is not null)
        {
        existingDirector.Name = directorDTO.Name;
        existingDirector.Age = directorDTO.Age;
        existingDirector.Bio = directorDTO.Bio;
        existingDirector.ProfilePictureUrl = directorDTO.ProfilePictureUrl;
        existingDirector.NumberOfAwards = directorDTO.NumberOfAwards;

        await _context.SaveChangesAsync();
      }
      }

      public async Task Delete(int id)
      {
        var directorToDelete = await GetByID(id);

        if(directorToDelete is not null)
        {
          _context.Directors.Remove(directorToDelete);
          await _context.SaveChangesAsync();
        }
      }

      public async Task<int> GetCount()
      {
        return await _context.Directors.CountAsync();
      }

      public async Task<bool> IsBrandNameUnique(string directorName)
      {
      var directors = await _context.Directors.AsNoTracking().ToListAsync();
      return directors.Any(b => string.Equals(b.Name, directorName, StringComparison.OrdinalIgnoreCase));
      }
      }
}
