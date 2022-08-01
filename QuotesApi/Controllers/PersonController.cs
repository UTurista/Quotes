using Microsoft.AspNetCore.Mvc;
using Quotes.Api.Models;
using Quotes.Api.Services;
using Quotes.Core.Models;
using Quotes.Core.Services;
using Quotes.Core.Models;

namespace Quotes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IIdService idService;
        private readonly ITransformService transformService;
        private readonly IPersonService personService;

        public PersonController(IIdService idService, ITransformService transformService, IPersonService personService)
        {
            this.transformService = transformService;
            this.idService = idService;
            this.personService = personService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var decodedId = idService.Decode(id);
            var person = await personService.GetAsync(decodedId, cancellationToken).ConfigureAwait(false);
            if (person is null)
            {
                return NotFound();
            }

            var dto = transformService.Transform(person);
            return Ok(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ViewPerson>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(string? offset, CancellationToken cancellationToken = default)
        {
            var decodedOffset = string.IsNullOrEmpty(offset) ? 0 : idService.Decode(offset);
            var persons = await personService.GetAllAsync(decodedOffset, cancellationToken).ConfigureAwait(false);

            var dto = transformService.Transform(persons.Values);
            return Ok(new Segment<ViewPerson>
            {
                Size = persons.Size,
                Values = dto
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Edit(string id, EditPerson personModel, CancellationToken cancellationToken = default)
        {
            var decodedId = idService.Decode(id);
            var person = transformService.Transform(personModel);
            var result = await personService.EditAsync(decodedId, person, cancellationToken).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ViewPerson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreatePerson createPerson, CancellationToken cancellationToken = default)
        {
            var person = transformService.Transform(createPerson);
            var result = await personService.CreateAsync(person, cancellationToken).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                var dto = transformService.Transform(person);
                return CreatedAtAction(nameof(Get), new { id = person.Id }, dto);
            }

            if(result.Error == Error.DuplicatePersonName)
            {
                ModelState.AddModelError(nameof(createPerson.Name), "Must be unique");
                return BadRequest(ModelState);
            }

            ModelState.AddModelError("", result.Error!.ToString());
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            var decodedId = idService.Decode(id);
            var result = await personService.DeleteAsync(decodedId, cancellationToken).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}