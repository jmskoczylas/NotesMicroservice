using Application.DTOs;
using Application.Commands;
using AutoMapper;
using Domain.Interfaces;
using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handler for updating a note.
    /// </summary>
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Result<NoteDto>>
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteCommandHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> to interact with the notes.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public UpdateNoteCommandHandler(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<NoteDto>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = _mapper.Map<INote>(request.Note);
            var result = await _noteRepository.UpdateAsync(note);

            return result.ToResult(_ => _mapper.Map<NoteDto>(result.ValueOrDefault));
        }
    }
}
