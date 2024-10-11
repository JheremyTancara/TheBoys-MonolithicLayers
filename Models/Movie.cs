namespace Api.Models;

public class Movie
{
    public int MovieID { get; set; } 
    public string Title { get; set; } = string.Empty;    
    public Genre Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; } 
    public Rating Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Actor>? Cast { get; set; }
    public Director? Director { get; set; } 
    public string TrailerUrl { get; set; } = string.Empty;
}
