using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quotes.Core.Models
{
    public class Quote
    {
        /// <summary>
        /// The unique identifier of this quote.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The sentence, phrase, or passage from speech or text that someone has said or written.
        /// </summary>
        public string Quotation { get; set; } = string.Empty;

        /// <summary>
        /// If set, the date when this quote was first said/written.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// The identifier of the person that has said/wrote this quote.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// The person that has said/wrote this quote.
        /// </summary>
        [ForeignKey(nameof(PersonId))]
        public virtual Person? Person { get; set; }
    }
}
