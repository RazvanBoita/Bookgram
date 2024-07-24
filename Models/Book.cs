using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearnByDoing.Validators;

namespace LearnByDoing.Models;

public partial class Book
{
    [Key]
    public int? BookId { get; set; }
    
    [StringLength(maximumLength:50 , ErrorMessage = "Title length must be between 4 and 50 characters", MinimumLength = 4)]
    public string? Title { get; set; }

    [StringLength(maximumLength:50 , ErrorMessage = "Authors length must be between 4 and 50 characters", MinimumLength = 4)]
    public string? Authors { get; set; }
    
    public double? AverageRating { get; set; }

    public string? Isbn { get; set; }

    public double? Isbn13 { get; set; }
    
    public string? LanguageCode { get; set; }

    [Range(50,1000, ErrorMessage = "NumPages has to be between 50 and 1000")]
    public int? NumPages { get; set; }

    
    public int? RatingsCount { get; set; }

    
    public int? TextReviewsCount { get; set; }

    [ValidatePastDate(ErrorMessage = "Date can't be later than today.")]    
    public string? PublicationDate { get; set; }

    public string? Publisher { get; set; }

    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
    public ICollection<WishlistUserBook> WishlistUserBooks { get; set; } = new List<WishlistUserBook>();
}
