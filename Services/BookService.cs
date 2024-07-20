using LearnByDoing.Data;
using LearnByDoing.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnByDoing.Services;

public class BookService : IBookService
{
    private const int BooksToGetCount = 10;
    private readonly ApplicationDbContext _context;
    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Book>> GetMainBooks()
    {
        var bestBooks = await _context.Books
            .Where(b => b.RatingsCount >= 20)
            .OrderByDescending(b => b.AverageRating)
            .Take(BooksToGetCount)
            .ToListAsync();
        return bestBooks;
    }
}