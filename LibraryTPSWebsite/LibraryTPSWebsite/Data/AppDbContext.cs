using LibraryTPSWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryTPSWebsite.Data
    {
    public class AppDbContext : DbContext
        {
        public AppDbContext( DbContextOptions<AppDbContext> options ) : base(options)
            {
            }
        protected override void OnModelCreating( ModelBuilder modelBuilder )
            {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<shelf>()
                .HasMany(x => x.books)
                .WithOne(x => x.shelf)
                .HasForeignKey(x => x.shelfID);
            modelBuilder.Entity<shelf>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<book>()
             .HasIndex(x => x.BookName)
            .IsUnique();
            }
      public DbSet<shelf> shelves { get; set; }
        public DbSet<book> books { get; set; }
        public DbSet<bookHistory> booksHistory { get; set; }
  
        }
    }
