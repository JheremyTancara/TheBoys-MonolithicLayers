using System.Text.Json.Serialization;
using Api.Models;

namespace Api.DTOs

{
    public class UserDTO
    {
        [JsonIgnore]
        public int UserID { get; set; }

        public string Username { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string SubscriptionLevel { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
    }
}
