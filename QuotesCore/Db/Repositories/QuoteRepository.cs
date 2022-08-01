using Microsoft.EntityFrameworkCore;
using Quotes.Core.Models;

namespace Quotes.Core.Db.Repositories
{
    /// <summary>
    /// A contract for the repository that will handle the logic of saving/retrieving the entity <see cref="Quote"/> from the database.
    /// </summary>
    public interface IQuoteRepository
    {
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<Quote?> GetAsync(int id, CancellationToken cancellationToken);
    }

    /// <summary>
    /// An implementation of <see cref="IQuoteRepository"/> using <see cref="QuoteDbContext"/>.
    /// </summary>
    public class QuoteRepository : IQuoteRepository
    {
        private readonly QuoteDbContext context;

        public QuoteRepository(QuoteDbContext context)
        {
            this.context = context;
        }

        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return context.Quotes.CountAsync(cancellationToken);
        }

        public Task<Quote?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return context.Quotes.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
