using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnByDoing.Models;

public partial class Book
{
    public int? BookId { get; set; }

    public string? Title { get; set; }

    public string? Authors { get; set; }

    
    [Column("average_rating")]
    public double? AverageRating { get; set; }

    public string? Isbn { get; set; }

    public double? Isbn13 { get; set; }
    
    [Column("language_code")]
    public string? LanguageCode { get; set; }

    
    [Column("num_pages")]
    public int? NumPages { get; set; }

    
    [Column("ratings_count")]
    public int? RatingsCount { get; set; }

    
    [Column("text_reviews_count")]
    public int? TextReviewsCount { get; set; }

    
    [Column("publication_date")]
    public string? PublicationDate { get; set; }

    public string? Publisher { get; set; }
}
