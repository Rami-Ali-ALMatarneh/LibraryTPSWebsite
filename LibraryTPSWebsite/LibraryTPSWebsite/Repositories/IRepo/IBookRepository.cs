using LibraryTPSWebsite.Models;

namespace LibraryTPSWebsite.Repositories.IRepo
    {
    public interface IBookRepository
        {
        Task<IEnumerable<book>> GetAllBookAsync();
        Task<book> GetBookByIdAsync(int id);
        Task AddBookAsync( book book );
        Task UpdateBookAsync(book book);
        Task DeleteBookAsync(int id);
        Task<book> GetBookByNameAsync( string name );
        }
    }
