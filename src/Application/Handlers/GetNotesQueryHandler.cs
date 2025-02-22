using Application.DTOs;
using Application.Querries;
using AutoMapper;
using Domain.Interfaces;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handler for getting paged notes.
    /// </summary>
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, Result<IReadOnlyCollection<NoteDto>>>
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotesQueryHandler"/> class.
        /// </summary>
        /// <param name="noteRepository">An instance of <see cref="INoteRepository"/> to interact with the notes.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="ArgumentNullException">noteRepository</exception>
        public GetNotesQueryHandler(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<Result<IReadOnlyCollection<NoteDto>>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            if (request.Page == 0)
            {
                request.Page = 1;
            }

            var result = await _noteRepository.GetAsync(request.Page, request.PageSize);
            if (result.IsFailed)
            {
                return result.ToResult();
            }

            IReadOnlyCollection<NoteDto> notes = result.ValueOrDefault.Select(note => this._mapper.Map<NoteDto>(note)).ToList();
            return notes.ToResult();
        }
    }
}
