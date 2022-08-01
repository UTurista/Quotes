using HashidsNet;

namespace Quotes.Api.Services
{
    /// <summary>
    /// A contract for the service responsible to encode/decode our incremental identifiers
    /// to a pseudo-random identifier.
    /// </summary>
    public interface IIdService
    {
        /// <summary>
        /// Converts from the encoded id to our domain.
        /// </summary>
        /// <param name="encoded">The encoded id.</param>
        /// <returns>The decoded id.</returns>
        int Decode(string encoded);


        /// <summary>
        /// Converts an in from our domain to an encoded form.
        /// </summary>
        /// <param name="id">The id as per our domain.</param>
        /// <returns>The encoded id.</returns>
        string Encode(int id);
    }

    /// <summary>
    /// An implementation of <see cref="IIdService"/> using the
    /// library https://github.com/ullmark/hashids.net
    /// </summary>
    public class IdService : IIdService
    {
        private readonly Hashids hasher;

        public IdService(IConfiguration configuration)
        {
            var salt = configuration.GetValue<string>("Identifier:Salt");
            var length = configuration.GetValue<int>("Identifier:Length");
            hasher = new(salt, length);
        }

        public int Decode(string encoded)
        {
            return hasher.DecodeSingle(encoded);
        }

        public string Encode(int id)
        {
            return hasher.Encode(id);
        }
    }
}
