using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quotes.Core.Db.Repositories;
using Quotes.Core.Models;

namespace Quotes.Core.Db
{
    public interface IQuoteDbUnitOfWork
    {
        public IPersonRepository Persons { get; }

        public IQuoteRepository Quotes { get; }

        public Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class QuoteDbUnitOfWork : IQuoteDbUnitOfWork
    {
        private readonly QuoteDbContext context;
        private readonly ILogger<QuoteDbUnitOfWork> logger;

        public IPersonRepository Persons { get; init; }
        public IQuoteRepository Quotes { get; init; }

        public QuoteDbUnitOfWork(QuoteDbContext context, ILogger<QuoteDbUnitOfWork> logger)
        {
            this.context = context;
            this.logger = logger;
            // Not the most correct to hardcode the types but
            // using DI would result in a different context for each repository
            Persons = new PersonRepository(context);
            Quotes = new QuoteRepository(context);
        }

        public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return Result.Sucess();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException sqlException)
                {
                    if (IsUniqueConstraintError(sqlException, "Persons.Name"))
                    {
                        return Result.Fail(Error.DuplicatePersonName);
                    }
                }
                logger.LogCritical(ex, "Unable to save database changes");
                return Result.Fail(Error.GenericError);
            }
        }

        private static bool IsUniqueConstraintError(SqliteException sqlException, string constraintName)
        {
            return  sqlException.SqliteErrorCode == 19 && sqlException.SqliteExtendedErrorCode == 2067 && sqlException.Message.Contains(constraintName);
        }
    }
}
