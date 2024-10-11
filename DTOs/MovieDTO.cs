using System.Text.Json.Serialization;
using Api.Models;

namespace Api.DTOs

{
    public class MovieDTO
    {
        [JsonIgnore]
        public int MovieID { get; set; }

        public string Title { get; set; } = string.Empty;    
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; } 
        public Rating Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> Cast { get; set; } = new List<string>(); 
        public string Director { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
    }
}
