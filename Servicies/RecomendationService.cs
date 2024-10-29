using Api.Models;

namespace Api.Services
{
    public class RecommendationService
    {
        private readonly List<Movie> _allMovies;

        public RecommendationService(List<Movie> allMovies)
        {
            _allMovies = allMovies; 
        }

        public List<Movie> GetRecommendedMovies(UserMovie user, ContentType preferredContentType)
        {
            var userPreferredGenres = user.WatchedMovies
                .SelectMany(m => m.Genre ?? new List<Genre>())
                .Concat(user.Watchlist.SelectMany(m => m.Genre ?? new List<Genre>()))
                .Distinct()
                .ToList();

            if (!userPreferredGenres.Any())
                return new List<Movie>();

            var recommendedMovies = _allMovies
                .Where(m => 
                    m.Type == preferredContentType 
                )
                .OrderByDescending(m => m.Views) 
                .ThenByDescending(m => m.Rating)
                .ToList();

            return recommendedMovies; 
        }

        public List<Movie> GetMostViewedMovies()
        {
            var mostViewedMovies = _allMovies
                .OrderByDescending(m => m.Views) 
                .ToList();

            return mostViewedMovies; 
        }
    }
}
