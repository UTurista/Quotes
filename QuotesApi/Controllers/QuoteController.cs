using Microsoft.AspNetCore.Mvc;
using Quotes.Api.Models;
using Quotes.Api.Services;
using Quotes.Core.Services;

namespace Quotes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly ITransformService transformService;
        private readonly IQuoteService quoteService;

        public QuoteController(ITransformService transformService, IQuoteService quoteService)
        {
            this.transformService = transformService;
            this.quoteService = quoteService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewQuote), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandom(CancellationToken cancellationToken = default)
        {
            var quote = await quoteService.GetRandomAsync(cancellationToken).ConfigureAwait(false);
            if (quote is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var dto = transformService.Transform(quote);
            return Ok(dto);
        }
    }
}