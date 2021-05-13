using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Models;

namespace Application.Mappings.Converters
{
    /// <summary>
    /// Converts an instance of a <see cref="NoteDto"/> to an <see cref="INote"/> instance.
    /// </summary>
    public class NoteDtoToNoteConverter : ITypeConverter<NoteDto, INote>
    {
        /// <summary>Performs conversion of a <see cref="NoteDto"/> to an <see cref="INote"/> instance.</summary>
        /// <param name="source">An instance of <see cref="INote"/>.</param>
        /// <param name="destination">An instance of <see cref="NoteDto"/>.</param>
        /// <param name="context">Resolution context</param>
        /// <returns>An instance of <see cref="INote"/> with values transferred over from source class.</returns>
        public INote Convert(NoteDto source, INote destination, ResolutionContext context) =>
            new Note(
                source.Id, 
                source.Title, 
                source.Text,
                source.CreatedOn, 
                source.ModifiedOn);
    }
}