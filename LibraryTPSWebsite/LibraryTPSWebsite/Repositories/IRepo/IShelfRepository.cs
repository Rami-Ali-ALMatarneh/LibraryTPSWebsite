using LibraryTPSWebsite.Models;

namespace LibraryTPSWebsite.Repositories.IRepo
    {
    public interface IShelfRepository
        {
        Task<IEnumerable<shelf>> GetAllShelfAsync();
        Task<shelf> GetShelfByIdAsync( int id );
        Task AddShelfAsync( shelf shelf );
        Task UpdateShelfAsync( shelf shelf );
        Task DeleteShelfAsync( int id );
        Task<shelf> GetShelfNameAsync( string name );
        Task<IEnumerable<book>> GetAllShelfWithBookAsync( int id );
        }
    }
