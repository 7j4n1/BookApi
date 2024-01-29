using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi
{
    public class LibraryDBContext(DbContextOptions<LibraryDBContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
        }
    }
}
