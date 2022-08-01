using System.ComponentModel.DataAnnotations;

namespace Quotes.Api.Models
{
    public class ViewPerson
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }

    public class CreatePerson
    {
        [Required]
        public string Name { get; init; } = string.Empty;
    }

    public class EditPerson
    {
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}
