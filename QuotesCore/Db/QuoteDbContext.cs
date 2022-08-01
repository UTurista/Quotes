using Microsoft.EntityFrameworkCore;
using Quotes.Core.Models;

namespace Quotes.Core.Db
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext() : base()
        {
        }

        public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons => Set<Person>();

        public DbSet<Quote> Quotes => Set<Quote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
