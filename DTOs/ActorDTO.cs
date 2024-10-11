using System.Text.Json.Serialization;
namespace Api.Models.DTOs

{
    public class ActorDTO
    {
        [JsonIgnore]
        public int ActorID { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
