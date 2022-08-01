using Microsoft.EntityFrameworkCore;
using Quotes.Core.Models;

namespace Quotes.Core.Db.Repositories
{
    /// <summary>
    /// A contract for the repository that will handle the logic of saving/retrieving the entity <see cref="Person"/> from the database.
    /// </summary>
    public interface IPersonRepository
    {
        void Add(Person person);
        void Delete(Person person);
        Task<Person?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all persons after the given index.
        /// </summary>
        /// <param name="id">The id of the previous call.</param>
        /// <param name="cancellationToken">The token to cancel this operation.</param>
        /// <returns></returns>
        Task<IEnumerable<Person>> GetAll(int id, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An implementation of <see cref="IPersonRepository"/> using <see cref="QuoteDbContext"/>.
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly QuoteDbContext context;

        public PersonRepository(QuoteDbContext context)
        {
            this.context = context;
        }

        public void Add(Person person)
        {
            context.Persons.Add(person);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return context.Persons.CountAsync(cancellationToken);
        }

        public void Delete(Person person)
        {
            context.Persons.Remove(person);
        }

        public async Task<IEnumerable<Person>> GetAll(int id, CancellationToken cancellationToken = default)
        {
            return await context.Persons
                .Where(x => x.Id > id)
                .Take(2)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<Person?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return context.Persons.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
