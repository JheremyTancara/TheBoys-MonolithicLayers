using System.Text.Json.Serialization;
namespace Api.Models.DTOs

{
    public class DirectorDTO
    {
        [JsonIgnore]
        public int DirectorID { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public int NumberOfAwards { get; set; }
    }
}
