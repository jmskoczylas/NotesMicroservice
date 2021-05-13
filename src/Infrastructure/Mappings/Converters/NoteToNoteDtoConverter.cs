using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Models;

namespace Application.Mappings.Converters
{
    /// <summary>
    /// Converts an instance of <see cref="INote"/> to a <see cref="NoteDto"/>.
    /// </summary>
    public class NoteToNoteDtoConverter : ITypeConverter<INote, NoteDto>
    {
        /// <summary>Performs conversion of an <see cref="INote"/> instance to a <see cref="NoteDto"/>.</summary>
        /// <param name="source">An instance of <see cref="INote"/>.</param>
        /// <param name="destination">An instance of <see cref="NoteDto"/>.</param>
        /// <param name="context">Resolution context</param>
        /// <returns>An instance of <see cref="NoteDto"/> with values transferred over from source class.</returns>
        public NoteDto Convert(INote source, NoteDto destination, ResolutionContext context)
        {
            return new()
            {
                CreatedOn = source.CreatedOn,
                ModifiedOn = source.ModifiedOn,
                Text = source.Text,
                Id = source.Id,
                Title = source.Title
            };
        }
    }
}
