using System.Security.Claims;
using LearnByDoing.Data;
using LearnByDoing.Models;
using LearnByDoing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace LearnByDoing.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IBookService _bookService;

    public BooksController(ILogger<BooksController> logger, ApplicationDbContext context, IBookService bookService)
    {
        _logger = logger;
        _context = context;
        _bookService = bookService;
    } 
    
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var firstTenBooks = await _bookService.GetMainBooks();
        return View(firstTenBooks);
    }
    
    public async Task<IActionResult> Details([FromQuery] int bookID)
    {
        var searchedBook = await _context.Books.FindAsync(bookID);
        return View(searchedBook);
    }

    public async Task<IActionResult> Search([FromQuery] String query)
    {
        if (query is null or "")
        {
            return View();
        }
        var results = await _context.Books
            .Where(b => (!string.IsNullOrEmpty(b.Title) && EF.Functions.Like(b.Title.ToLower(), $"%{query}%")) ||
                        (!string.IsNullOrEmpty(b.Authors) && EF.Functions.Like(b.Authors.ToLower(), $"%{query}%")))
            .ToListAsync();
        
        return View(results);
    }

    [HttpPost]
    public async Task<IActionResult> AddToRead(int BookID)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }
    
        var userBook = new UserBook()
        {
            BookId = BookID,
            UserId = currentUserId
        };

        if (_context.UserBooks.Any(ub => ub.BookId == BookID && ub.UserId == currentUserId))
        {
            return BadRequest("Book already added");
        }
        
        _context.UserBooks.Add(userBook);
        await _context.SaveChangesAsync();
        return Ok("Book added successfully");
    }

    public async Task<IActionResult> Finished()
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }
        
        var entriesReadByCurrentUser = await _context.UserBooks.Where(ub => ub.UserId == currentUserId).ToListAsync();
        ICollection<Book> booksReadByCurrentUser = new List<Book>();
        foreach (var entry in entriesReadByCurrentUser)
        {
            var book = await _context.Books.FindAsync(entry.BookId);
            if (book is not null)
            {
                booksReadByCurrentUser.Add(book);
            }
        }

        return View(booksReadByCurrentUser);
    }
}