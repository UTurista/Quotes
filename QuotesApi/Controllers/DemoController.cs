using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quotes.Core.Db;
using Quotes.Core.Models;

namespace QuotesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly QuoteDbContext context;

        public DemoController(QuoteDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            await context.Database.MigrateAsync(cancellationToken);
            return Ok();

        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed(CancellationToken cancellationToken = default)
        {
            await AddQuotes("Alice", new[]
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                "Vestibulum euismod urna ac sem molestie, eu venenatis libero congue.",
                "Duis tristique velit ac tortor vulputate, sed venenatis nunc dignissim.",
                "Integer sed arcu quis dui commodo rutrum non ut arcu.",
                "In pretium felis sed elit tincidunt pulvinar."
            });
            await AddQuotes("Bob", new[]
            {
                "Fusce malesuada lacus feugiat urna fermentum ornare.",
                "Nunc in augue congue, suscipit risus quis, blandit sem.",
                "Nulla ut orci convallis, pharetra risus quis, semper neque.",
                "Mauris blandit metus pharetra, pellentesque eros quis, lacinia turpis.",
                "Suspendisse accumsan odio nec odio scelerisque lobortis."
            });
            await AddQuotes("Erika", new[]
           {
                "Sed sodales enim in augue pellentesque, ut volutpat eros mattis.",
                "Sed iaculis neque non dolor condimentum rutrum.",
                "Phasellus non diam eget est molestie finibus in ac ex.",
                "Nunc at nunc tempor, ornare sapien a, placerat ex."
            });
            return Ok();

        }

        private async  Task AddQuotes(string name, string[] quotes, CancellationToken cancellationToken = default)
        {
            Person person = new()
            {
                Name = name,
            };
            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var quote in quotes)
            {
                var randomDate = TimeSpan.FromDays(new Random().Next(4000));
                context.Quotes.Add(new Quote
                {
                    Quotation = quote,
                    Date = DateTime.UtcNow.Subtract(randomDate),
                    PersonId = person.Id
                });
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken = default)
        {
            await context.Database.EnsureDeletedAsync(cancellationToken);
            return Ok();
        }
    }
}
