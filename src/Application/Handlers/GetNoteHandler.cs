using Application.Requests;
using AutoMapper;
using FluentResults;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handler for getting note.
    /// </summary>
    public class GetNoteHandler : IRequestHandler<GetNoteRequest, Result<NoteDto>>
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNoteHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> to interact with the notes.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public GetNoteHandler(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<NoteDto>> Handle(GetNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteRepository.GetAsync(request.Id);
            return result.ToResult(_ => _mapper.Map<NoteDto>(result.ValueOrDefault));
        }
    }
}
