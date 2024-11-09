using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Converters
{
    /// <summary>
    /// Converts an instance of <see cref="StandardNote"/> to <see cref="NoteDto"/>.
    /// </summary>
    public class StandardNoteToNoteDtoConverter : ITypeConverter<StandardNote, NoteDto>
    {
        /// <summary>
        /// Converts a <see cref="StandardNote"/> to a <see cref="NoteDto"/>.
        /// </summary>
        /// <param name="source">The source <see cref="StandardNote"/> instance.</param>
        /// <param name="destination">The target <see cref="NoteDto"/> instance.</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>A <see cref="NoteDto"/> populated with values from the source.</returns>
        public NoteDto Convert(StandardNote source, NoteDto destination, ResolutionContext context) =>
            new()
            {
                Id = source.Id,
                Title = source.Title,
                Text = source.Text,
                CreatedOn = source.CreatedOn,
                ModifiedOn = source.ModifiedOn
            };
    }
}
