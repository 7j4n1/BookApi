using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi
{
    /// <summary>
    /// Represents the database context for the library application.
    /// </summary>
    public class LibraryDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDBContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the collection of books in the library.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets the collection of authors in the library.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets the collection of publishers in the library.
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// Configures the model for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder to use for configuration.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
        }
    }
}
