using Quotes.Core.Db;
using Quotes.Core.Models;

namespace Quotes.Core.Services
{
    public interface IQuoteService
    {
        Task<Quote?> GetRandomAsync(CancellationToken cancellationToken);
    }
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteDbUnitOfWork quoteUoW;

        public QuoteService(IQuoteDbUnitOfWork quoteUoW)
        {
            this.quoteUoW = quoteUoW;
        }

        public async Task<Quote?> GetRandomAsync(CancellationToken cancellationToken)
        {
            var max = await quoteUoW.Quotes.CountAsync(cancellationToken).ConfigureAwait(false);
            var rand = new Random().Next(1, max);
            return await quoteUoW.Quotes.GetAsync(rand, cancellationToken).ConfigureAwait(false);
        }
    }
}
