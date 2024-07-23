namespace LearnByDoing.Models;

public class WishlistUserBook
{
    public string? UserId { get; set; }
    public AspNetUser User { get; set; } = null!;
    
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}