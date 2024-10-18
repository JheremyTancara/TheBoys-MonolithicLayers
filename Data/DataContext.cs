using System.Text.Json;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Director> Directors => Set<Director>();

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public async Task SeedData()
        {
            await SeedEntity<Actor>("Data/actors.json", Actors);
            await SeedEntity<Director>("Data/directors.json", Directors);
        }

        private async Task SeedEntity<T>(string filePath, DbSet<T> dbSet) where T : class
        {
            if (!dbSet.Any())
            {
                var jsonData = await File.ReadAllTextAsync(filePath);
                var entities = JsonSerializer.Deserialize<List<T>>(jsonData);

                if (entities != null && entities.Any())
                {
                    await dbSet.AddRangeAsync(entities);
                    await SaveChangesAsync();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Watchlist)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserWatchlist")); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.RecommendedMovies)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserRecommendedMovies"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.WatchedMovies)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserWatchedMovies")); 
        }
    }
}
