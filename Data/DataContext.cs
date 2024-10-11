using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Movie> Movies => Set<Movie>();

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
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
