using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Operations;

namespace LearnByDoing.Models;

public class Review
{
    [Key]
    public int? ReviewId { get; set; }
    
    [Range(0,6, ErrorMessage = "Rating must be between 0 and 5 stars.")]
    public int? Stars { get; set; }
    
    [StringLength(100, ErrorMessage = "Review content can't exceed 100 characters.")]
    public string? Content { get; set; }
    
    
    public int? BookId { get; set; }
    public Book? Book { get; set; }
    
    public string? UserId { get; set; }
    public AspNetUser? User { get; set; }
    
    
}