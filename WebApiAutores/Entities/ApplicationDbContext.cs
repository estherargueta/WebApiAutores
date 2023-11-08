using Microsoft.EntityFrameworkCore;

namespace WebApiAutores.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasIndex(x => x.ISBN)
                .IsUnique(true);
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
