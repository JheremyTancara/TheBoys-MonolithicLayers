using System.Text.Json.Serialization;
using Api.Models;

namespace Api.DTOs

{
    public class MovieDTO
    {
        [JsonIgnore]
        public int MovieID { get; set; }

        public string Title { get; set; } = string.Empty;    
        public string Genre { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public int Duration { get; set; } 
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
    }
}
