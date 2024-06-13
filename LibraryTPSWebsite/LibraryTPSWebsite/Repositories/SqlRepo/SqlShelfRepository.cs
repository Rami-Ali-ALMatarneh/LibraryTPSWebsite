using LibraryTPSWebsite.Data;
using LibraryTPSWebsite.Models;
using LibraryTPSWebsite.Repositories.IRepo;
using Microsoft.EntityFrameworkCore;

namespace LibraryTPSWebsite.Repositories.SqlRepo
    {
    public class SqlShelfRepository : IShelfRepository
        {
        private readonly AppDbContext appDbContext;

        public SqlShelfRepository( AppDbContext appDbContext )
            {
            this.appDbContext = appDbContext;
            }
        public async Task AddShelfAsync( shelf shelf )
            {
            await appDbContext.shelves.AddAsync( shelf );
            await appDbContext.SaveChangesAsync();
            }

        public async Task DeleteShelfAsync( int id )
            {
            var shelf = await appDbContext.shelves.FindAsync(id);
            if (shelf != null)
                {
                if (shelf.IsDeleted)
                    {
                    appDbContext.shelves.Remove(shelf);
                    await appDbContext.SaveChangesAsync();
                    }
                }
            }

        public async Task<IEnumerable<shelf>> GetAllShelfAsync()
            {
            return  await appDbContext.shelves.Include(x=>x.books).ToListAsync();
            }

        public async Task<IEnumerable<book>> GetAllShelfWithBookAsync( int id)
            {
            return await appDbContext.books
                .Where(x=>x.shelfID==id)
                .ToListAsync();
            }

        public async Task<shelf> GetShelfByIdAsync( int id )
            {
            return await appDbContext.shelves.Include(x=>x.books).SingleOrDefaultAsync(x=>x.shelfID==id);
            }

        public async Task<shelf> GetShelfNameAsync( string name )
            {
            return await appDbContext.shelves.FirstOrDefaultAsync(x => x.Name == name);
            }

        public async Task UpdateShelfAsync( shelf shelf )
            {
            var Shelf = appDbContext.shelves.Attach(shelf);
            Shelf.State = EntityState.Modified;
           await appDbContext.SaveChangesAsync();
            }
        }
    }
