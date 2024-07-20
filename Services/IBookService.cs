using LearnByDoing.Models;

namespace LearnByDoing.Services;

public interface IBookService
{
     Task<IEnumerable<Book>> GetMainBooks();
}