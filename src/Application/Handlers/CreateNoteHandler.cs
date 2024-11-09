using Application.DTOs;
using Application.Requests;
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
    /// Handler for creating a note.
    /// </summary>
    public class CreateNoteHandler : IRequestHandler<CreateNoteRequest, Result<NoteDto>>
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteNoteHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> used to perform operations on notes micro service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public CreateNoteHandler(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<NoteDto>> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = _mapper.Map<INote>(request.Note);
            var result = await _noteRepository.CreateAsync(note);

            return result.ToResult(_ =>_mapper.Map<NoteDto>(result.ValueOrDefault));
        }
    }
}
