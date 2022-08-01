namespace Quotes.Core.Models
{
    /// <summary>
    /// Represents a sub-set of items and the size of the entire collection.
    /// </summary>
    /// <typeparam name="T">The type of the items represented.</typeparam>
    public class Segment<T>
    {
        /// <summary>
        /// The actual sub-set items.
        /// </summary>
        public IEnumerable<T> Values { get; init; } = Enumerable.Empty<T>();

        /// <summary>
        /// The size of the entire collection.
        /// </summary>
        public int Size { get; init; }
    }
}
