using Application.Requests;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    /// <summary>
    /// API controller used for performing CRUD operations and archiving a note.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator used to initialize individual API endpoints.</param>
        /// <exception cref="ArgumentNullException">
        /// noteManager
        /// or
        /// mapper
        /// or
        /// identityProvider
        /// </exception>
        public NoteController(IMediator mediator) 
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Creates the specified note.
        /// </summary>
        /// <param name="noteDto">The note dto to create.</param>
        /// <returns>An instance of the note with create details.</returns>
        /// <response code="200">Successfully created a note.</response>
        /// <response code="409">Note with this name already exists.</response>
        /// <response code="401">Failed to authorize user identity.</response>
        /// <response code="400">Failed to create a note.</response>
        [ProducesResponseType(typeof(NoteDto),201)]
        [ProducesResponseType(typeof(int),409)]
        [ProducesResponseType(typeof(int), 401)]
        [ProducesResponseType(typeof(int),400)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteDto noteDto)
        {
            var result = await _mediator.Send(new CreateNoteRequest(noteDto));
            return result.IsFailed ? this.BadRequest(result.ToResult()) : this.Created(result.Value.Id.ToString(), result.ValueOrDefault);
        }

        /// <summary>
        /// Deletes the specified note.
        /// </summary>
        /// <param name="id">The note identifier.</param>
        /// <returns>Appropriate response code alone, depending on an operation result.</returns>
        /// <response code="204">Successfully deleted a note.</response>
        /// <response code="401">Failed to authorize user identity.</response>
        /// <response code="400">Failed to delete a note.</response>
        [ProducesResponseType(typeof(int),204)]
        [ProducesResponseType(typeof(int), 401)]
        [ProducesResponseType(typeof(int), 400)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNoteRequest(id));
            return result.IsSuccess ? this.NoContent() : this.BadRequest();
        }

        /// <summary>
        /// Gets an instance of a note by specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the note to get.</param>
        /// <returns>An instance of the note with specified identifier.</returns>
        /// <response code="200">Successfully returned a note.</response>
        /// <response code="404">Note could not be found.</response>
        /// <response code="401">Failed to authorize user identity.</response>
        /// <response code="400">Failed to get a note.</response>
        [ProducesResponseType(typeof(NoteDto), 200)]
        [ProducesResponseType(typeof(int), 401)]
        [ProducesResponseType(typeof(int), 400)]
        [ProducesResponseType(typeof(int), 404)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetNoteRequest(id));
            if (result == null) return new NotFoundResult();
            return this.Ok(result.ValueOrDefault);
        }

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <returns>A paged list of all notes.</returns>
        /// <response code="200">Successfully returned a note.</response>
        /// <response code="404">Note could not be found.</response>
        /// <response code="401">Failed to authorize user identity.</response>
        /// <response code="400">Failed to get notes.</response>
        [ProducesResponseType(typeof(NoteDto), 200)]
        [ProducesResponseType(typeof(int), 401)]
        [ProducesResponseType(typeof(int), 400)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize, [FromQuery] int page)
        {
            var result = await _mediator.Send(new GetNotesRequest(page, pageSize));
            return result.IsFailed ? this.BadRequest() : this.Ok(result.ValueOrDefault);
        }

        /// <summary>
        /// Updates the specified note.
        /// </summary>
        /// <param name="noteDto">The note dto to update.</param>
        /// <returns>An instance of the note with updated details.</returns>
        /// <response code="200">Successfully updated a note.</response>
        /// <response code="409">Note cannot be updated, item with this name already exists.</response>
        /// <response code="401">Failed to authorize user identity.</response>
        /// <response code="400">Failed to update a note.</response>
        [ProducesResponseType(typeof(NoteDto), 201)]
        [ProducesResponseType(typeof(int), 409)]
        [ProducesResponseType(typeof(int), 401)]
        [ProducesResponseType(typeof(int), 400)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NoteDto noteDto)
        {
            var result = await _mediator.Send(new UpdateNoteRequest(noteDto));
            return result.IsSuccess ? this.Ok(result.ValueOrDefault) : this.BadRequest();
        }
    }
}
