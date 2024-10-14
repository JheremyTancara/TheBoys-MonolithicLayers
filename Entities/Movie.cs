namespace Api.Models;

public class Movie
{
    public int MovieID { get; set; } 
    public string Title { get; set; } = string.Empty;    
    public List<Genre>? Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Duration { get; set; } 
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Actor>? Cast { get; set; }
    public Director? Director { get; set; } 
    public string ImageUrl { get; set; } = string.Empty;
    public string TrailerUrl { get; set; } = string.Empty;
    public ContentType Type { get; set; }
    public int Views { get; set; }
}
