using Quotes.Api.Models;
using Quotes.Core.Models;

namespace Quotes.Api.Services
{
    /// <summary>
    /// A contract for the service responsible to transform models between the domain and the QuoteApi's presentation layer.
    /// </summary>
    public interface ITransformService
    {
        public ViewQuote Transform(Quote quote);
        public ViewPerson Transform(Person person);
        public IEnumerable<ViewPerson> Transform(IEnumerable<Person> persons);
        public  Person Transform(CreatePerson createPerson);
        public  Person Transform(EditPerson editPerson);
    }

    /// <summary>
    /// A simple implementation of <see cref="ITransformService"/>
    /// </summary>
    public class TransformService : ITransformService
    {
        private readonly IIdService idService;

        public TransformService(IIdService idService)
        {
            this.idService = idService;
        }

        public ViewQuote Transform(Quote quote)
        {
            return new ViewQuote
            {
                Id = idService.Encode(quote.Id),
                Date = quote.Date,
                PersonId = idService.Encode(quote.PersonId),
                Quotation = quote.Quotation,
            };
        }

        public ViewPerson Transform(Person person)
        {
            return new ViewPerson
            {
                Id = idService.Encode(person.Id),
                Name = person.Name,
            };
        }

        public IEnumerable<ViewPerson> Transform(IEnumerable<Person> persons)
        {
            return persons.Select(x => Transform(x));
        }

        public Person Transform(CreatePerson createPerson)
        {
            return new Person
            {
                Name = createPerson.Name,
            };
        }

        public Person Transform(EditPerson editPerson)
        {
            return new Person
            {
                Name = editPerson.Name,
            };
        }
    }
}
