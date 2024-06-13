using LibraryTPSWebsite.Data;
using LibraryTPSWebsite.Models;
using LibraryTPSWebsite.Repositories.IRepo;
using Microsoft.EntityFrameworkCore;

namespace LibraryTPSWebsite.Repositories.SqlRepo
    {
    public class SqlBookRepository : IBookRepository
        {
        private readonly AppDbContext appDbContext;

        public SqlBookRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            }
        public async Task AddBookAsync( book book )
            {
            await appDbContext.books.AddAsync( book );
            await appDbContext.SaveChangesAsync();                
            var Book = await appDbContext.books.FirstOrDefaultAsync(x=>x.BookName == book.BookName);
            logBookHistory(Book.BookID, "INSERTED");
            await appDbContext.SaveChangesAsync();

            }

        public async Task DeleteBookAsync( int id )
            {
            var book = await appDbContext.books.FindAsync( id );
            if (book != null)
                {
                if( book.IsDeleted )
                    {
                    logBookHistory(book.BookID, "DELETED");
                    appDbContext.books.Remove( book );
                    await appDbContext.SaveChangesAsync();
                    }
                }
            }

        public async Task<IEnumerable<book>> GetAllBookAsync()
            {
            return await appDbContext.books.ToListAsync();
            }

        public async Task<book> GetBookByIdAsync( int id )
            {
            return await appDbContext.books.FindAsync(id);
            }

        public async Task<book> GetBookByNameAsync( string name )
            {
            return await appDbContext.books.FirstOrDefaultAsync(x => x.BookName == name);
            }

        public async Task UpdateBookAsync( book book )
            {
            logBookHistory(book.BookID, "UPDATED");
            appDbContext.Entry(book).State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();
            }

        private void logBookHistory( int bookId, string ChangeType )
            {
            var book =  appDbContext.books.Find(bookId);
            if (book != null)
                {
                var booKHistory = new bookHistory
                    {
                    BookID = book.BookID,
                    BookName = book.BookName,
                    shelfID = book.shelfID,
                    ChangeDate = DateTime.Now,
                    ChangeType = ChangeType
                    };
                appDbContext.booksHistory.Add(booKHistory);
                }
            }

        }
    }
