using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi
{
    public class LibraryDBContext(DbContextOptions<LibraryDBContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }
    }
}
