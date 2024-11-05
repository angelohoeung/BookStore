using System.Data.Entity;

namespace BookStoreLIB
{
    public class BookContext : DbContext
    {
        public BookContext() : base("name=BookStoreConnection")
        {
        }

        public DbSet<Book> Books { get; set; }
    }
} 