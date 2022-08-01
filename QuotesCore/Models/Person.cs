using System.ComponentModel.DataAnnotations;

namespace Quotes.Core.Models
{
    public class Person
    {
        /// <summary>
        /// The unique identifier of this person.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of this person.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The collection of quotes said/written by this person.
        /// </summary>
        public virtual IList<Quote> Quotes { get; set; } = new List<Quote>();
    }
}
