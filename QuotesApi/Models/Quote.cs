using System.ComponentModel.DataAnnotations;

namespace Quotes.Api.Models
{
    public class ViewQuote
    {
        public string Id { get; init; } = string.Empty;

        public DateTime? Date { get; init; }

        public string Quotation { get; init; } = string.Empty;

        public string  PersonId { get; init; } = string.Empty;
    }

    public class CreateQuote
    {
        public DateTime? Date { get; init; }

        [Required]
        public string Quotation { get; init; } = string.Empty;

        public string PersonId { get; init; } = string.Empty;
    }
}
