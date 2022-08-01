using Microsoft.Extensions.Logging;
using Quotes.Core.Db;
using Quotes.Core.Models;

namespace Quotes.Core.Services
{
    public interface IPersonService
    {
        Task<Person?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> CreateAsync(Person person, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Segment<Person>> GetAllAsync(int offset,  CancellationToken cancellationToken = default);
        Task<Result> EditAsync(int id, Person person, CancellationToken cancellationToken = default);
    }

    public class PersonService : IPersonService
    {
        private readonly IQuoteDbUnitOfWork quoteUoW;
        private readonly ILogger<PersonService> logger;

        public PersonService(IQuoteDbUnitOfWork quoteUoW, ILogger<PersonService> logger)
        {
            this.quoteUoW = quoteUoW;
            this.logger = logger;
        }

        public async Task<Result> CreateAsync(Person person, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(person.Name))
            {
                return Result.Fail(Error.GenericError);
            }

            quoteUoW.Persons.Add(person);
            await quoteUoW.SaveChangesAsync(cancellationToken);

            return Result.Sucess();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var person = await quoteUoW.Persons.GetAsync(id, cancellationToken).ConfigureAwait(false);
            if(person is null)
            {
                return Result.Sucess();
            }

            quoteUoW.Persons.Delete(person);
            await quoteUoW.SaveChangesAsync(cancellationToken);
            return Result.Sucess();
        }

        public Task<Result> EditAsync(int id, Person person, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Segment<Person>> GetAllAsync(int offset, CancellationToken cancellationToken = default)
        {
            var count = await quoteUoW.Persons.CountAsync(cancellationToken);
            var segment = await quoteUoW.Persons.GetAll(offset, cancellationToken);
            return new Segment<Person>
            {
                Size = count,
                Values = segment
            };
        }

        public Task<Person?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return quoteUoW.Persons.GetAsync(id, cancellationToken);
        }
    }
}
