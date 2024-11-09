using Application.DTOs;
using FluentResults;
using MediatR;
using System.Collections.Generic;

namespace Application.Requests
{
    /// <summary>
    /// Request object for getting paged notes.
    /// </summary>
    public class GetNotesRequest : IRequest<Result<IReadOnlyCollection<NoteDto>>>
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
        /// Initializes a new instance of the <see cref="GetNotesRequest"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public GetNotesRequest(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}
