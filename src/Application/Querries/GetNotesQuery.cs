using Application.DTOs;
using FluentResults;
using MediatR;
using System.Collections.Generic;

namespace Application.Querries
{
    /// <summary>
    /// Request object for getting paged notes.
    /// </summary>
    public class GetNotesQuery : IRequest<Result<IReadOnlyCollection<NoteDto>>>
    {
        /// <summary>
        /// Gets the page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotesQuery"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public GetNotesQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
