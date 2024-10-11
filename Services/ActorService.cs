using Api.Data;
using Api.DTOs;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services

{
    public class ActorService
    {
      private readonly DataContext _context;

      public ActorService(DataContext context)
      {
          _context = context;
      }

      public async Task<IEnumerable<Actor>> GetAll()
      {
        return await _context.Actors.ToListAsync();
      }

      public async Task<Actor?> GetByID(int id)
      {
        return await _context.Actors.FindAsync(id);
      }

      public async Task<Actor> Create(ActorDTO newActorDTO)
      {
        var newActor = new Actor();
        newActor.ActorID = await GetCount() + 1;
        newActor.Name = newActorDTO.Name;
        newActor.Age = newActorDTO.Age;
        newActor.Bio = newActorDTO.Bio;
        newActor.ProfilePictureUrl = newActorDTO.ProfilePictureUrl;

        _context.Actors.Add(newActor);
        await _context.SaveChangesAsync();

        return newActor;
      }

      public async Task Update(int id, ActorDTO actorDTO)
      {
        var existingActor = await GetByID(id);

        if (existingActor is not null)
        {
        existingActor.Name = actorDTO.Name;
        existingActor.Age = actorDTO.Age;
        existingActor.Bio = actorDTO.Bio;
        existingActor.ProfilePictureUrl = actorDTO.ProfilePictureUrl;

        await _context.SaveChangesAsync();
      }
      }

      public async Task Delete(int id)
      {
        var actorToDelete = await GetByID(id);

        if(actorToDelete is not null)
        {
          _context.Actors.Remove(actorToDelete);
          await _context.SaveChangesAsync();
        }
      }

      public async Task<int> GetCount()
      {
        return await _context.Actors.CountAsync();
      }

      public async Task<bool> IsBrandNameUnique(string actorName)
      {
      var actors = await _context.Actors.AsNoTracking().ToListAsync();
      return actors.Any(b => string.Equals(b.Name, actorName, StringComparison.OrdinalIgnoreCase));
      }
      }
}
