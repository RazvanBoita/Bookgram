using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnByDoing.Models;

public partial class Book
{
    [Key]
    public int? BookId { get; set; }

    public string? Title { get; set; }

    public string? Authors { get; set; }
    
    public double? AverageRating { get; set; }

    public string? Isbn { get; set; }

    public double? Isbn13 { get; set; }
    
    public string? LanguageCode { get; set; }

    
    public int? NumPages { get; set; }

    
    public int? RatingsCount { get; set; }

    
    public int? TextReviewsCount { get; set; }

    
    public string? PublicationDate { get; set; }

    public string? Publisher { get; set; }

    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
    public ICollection<WishlistUserBook> WishlistUserBooks { get; set; } = new List<WishlistUserBook>();
}
