using System.Security.Claims;
using LearnByDoing.Data;
using LearnByDoing.Models;
using LearnByDoing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<IdentityUser> _userManager;
    public BooksController(ILogger<BooksController> logger, ApplicationDbContext context, IBookService bookService, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _bookService = bookService;
        _userManager = userManager;
    } 
    
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var currentUserName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (currentUserName is null)
        {
            ViewData["hasPublished"] = false;
            Console.WriteLine("not showing because no username");
        }
        else
        {
            var hasPublished =  _context.Books.Where(b => b.Publisher == currentUserName).Any();
            Console.WriteLine("Has published is " + hasPublished);
            ViewData["hasPublished"] = hasPublished;
        }

        var firstTenBooks = await _bookService.GetMainBooks();
        return View(firstTenBooks);
    }
    
    public async Task<IActionResult> Details([FromQuery] int bookID)
    {
        var searchedBook = await _context.Books.FindAsync(bookID);
        var currentUserName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        var userIsAuthor = false;
        userIsAuthor = currentUserName == searchedBook?.Publisher;
        ViewData["isAuthor"] = userIsAuthor;
        
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userHasReviewedBook = _context.Reviews.Any(r => r.BookId == bookID && r.UserId == currentUserId);
        ViewData["hasReviewed"] = userHasReviewedBook;
        
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
        return RedirectToAction("Finished");
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

    public async Task<IActionResult> Unfinished([FromQuery] int bookID)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }   
        var searchedEntry = await _context.UserBooks.FindAsync(currentUserId, bookID);
        if (searchedEntry is null)
        {
            return BadRequest("Can't delete book, because it doesn't exist in Finished.");
        }

        _context.UserBooks.Remove(searchedEntry);
        await _context.SaveChangesAsync();
        return RedirectToAction("Finished");
    }
    public async Task<IActionResult> Wishlist()
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }

        var entriesFoundInWishlist = await _context.WishlistUserBooks.Where(ub => ub.UserId == currentUserId).ToListAsync();
        ICollection<Book> wishlistedBooks = new List<Book>();
        foreach (var entry in entriesFoundInWishlist)
        {
            var book = await _context.Books.FindAsync(entry.BookId);
            if (book is not null)
            {
                wishlistedBooks.Add(book);
            }
        }
        return View(wishlistedBooks);
    }
    
    public async Task<IActionResult> AddToWishlist(int BookID)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }

        var toAdd = new WishlistUserBook()
        {
            BookId = BookID,
            UserId = currentUserId
        };

        if (_context.WishlistUserBooks.Any(ub => ub.BookId == BookID && ub.UserId == currentUserId))
        {
            return BadRequest("Book already added to wishlist");
        }

        if (_context.UserBooks.Any(ub => ub.BookId == BookID && ub.UserId == currentUserId))
        {
            return BadRequest("It seems like you already read this book, hence we can't add it to wishlist.");
        }

        await _context.WishlistUserBooks.AddAsync(toAdd);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Wishlist");
    }

    public async Task<IActionResult> Unwish([FromQuery] int bookId)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }

        var searchedEntry = await _context.WishlistUserBooks.FindAsync(currentUserId, bookId);
        if (searchedEntry is null)
        {
            return BadRequest("No book to delete from wishlist.");
        }

        _context.WishlistUserBooks.Remove(searchedEntry);
        await _context.SaveChangesAsync();
        return RedirectToAction("Wishlist");
    }

    public async Task<IActionResult> MoveToRead([FromQuery] int bookId)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId is null)
        {
            return Unauthorized("User not found");
        }
        var searchedEntry = await _context.WishlistUserBooks.FindAsync(currentUserId, bookId);
        if (searchedEntry is null)
        {
            return BadRequest("No such book found in wishlist");
        }
        if (_context.UserBooks.Any(ub => ub.BookId == bookId && ub.UserId == currentUserId))
        {
            return BadRequest("Book already added to finished.");
        }

        await _context.UserBooks.AddAsync(new UserBook(){BookId = bookId, UserId = currentUserId});
        _context.WishlistUserBooks.Remove(searchedEntry);
        await _context.SaveChangesAsync();
        return RedirectToAction("Finished");
    }


    public IActionResult NewBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult NewBook(Book book)
    {
        
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        book.Isbn13 = 9780374520656;
        book.Isbn = "0374520658";
        book.AverageRating = 0;
        book.RatingsCount = 0;
        book.TextReviewsCount = 0;
        _context.Books.Add(book);
        _context.SaveChanges();
        return View("AllDone");
    }

    public IActionResult Delete(int BookId)
    {
        //mai facem un check, ca altfel orice user ar putea apela endpoint-ul asta si sa stearga orice carte
        Console.WriteLine(BookId);
        var bookToBeDeleted = _context.Books.Find(BookId);
        if (bookToBeDeleted is null)
        {
            return BadRequest("No book found with id " + BookId);
        }
        var currentUserName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (currentUserName is null)
        {
            return Unauthorized("User not found");
        }

        var isAuthor = currentUserName == bookToBeDeleted?.Publisher;
        if (!isAuthor)
        {
            return BadRequest("You don't have the privileges to delete book with id " + BookId);
        }

        _context.Books.Remove(bookToBeDeleted);
        _context.SaveChanges();
        return View("AllDone");
    }

    public IActionResult Published()
    {
        var currentUserName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (currentUserName is null)
        {
            return BadRequest("You haven't published any books yet...");
        }
        
        var publishedBooks =  _context.Books.Where(b => b.Publisher == currentUserName);
        if (publishedBooks.Any() is false)
        {
            return BadRequest("You haven't published any books yet...");
        }

        return View(publishedBooks);

    }
    
    public IActionResult Review(int BookId)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (_context.Reviews.Any(r => r.BookId == BookId && r.UserId == currentUserId))
        {
            return BadRequest("You already added a review for this page.");
        }
        return View(new Review(){BookId = BookId});
    }

    [HttpPost]
    public IActionResult Review(Review model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                "Review invalid. Stars should be between 0-5. Content length not more than 100 characters");
        }
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (_context.Reviews.Any(r => r.BookId == model.BookId && r.UserId == currentUserId))
        {
            return BadRequest("You already added a review for this page.");
        }

        model.UserId = currentUserId;
        try
        {
            _context.Reviews.Add(model);
            _context.SaveChanges();
            return View("AllDone");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public IActionResult ReviewDel([FromQuery] int BookId)
    {
        var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!_context.Reviews.Any(r => r.BookId == BookId && r.UserId == currentUserId))
        {
            return BadRequest("You don't have a review added for this book.");
        }

        var review = _context.Reviews.First(r => r.BookId == BookId && r.UserId == currentUserId);
        _context.Reviews.Remove(review);
        _context.SaveChanges();
        return View("AllDone");
    }

    [AllowAnonymous]
    public async Task<IActionResult> Reviews([FromQuery] int BookId)
    {
        var allReviews = await _context.Reviews.Where(r => r.BookId == BookId).ToListAsync();
        foreach (var review in allReviews)
        {
            Console.WriteLine("Am un review aici pt content " + review.Content);
            var user = await _userManager.FindByIdAsync(review.UserId);
            if (user is null)
            {
                continue;
            }
            review.UserId = user.UserName;
        }

        var book = await _context.Books.FindAsync(BookId);
        ViewData["bookTitle"] = book.Title;
        Console.WriteLine(allReviews.Count());
        ViewData["hasReviews"] = true;
        if (allReviews.Count() == 0)
        {
            Console.WriteLine("intru aici");
            ViewData["hasReviews"] = false;
        }
        return View(allReviews);
    }
    
}