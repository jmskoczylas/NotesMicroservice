using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Mappings.Converters
{
    /// <summary>
    /// Converts an instance of a <see cref="NoteEntity"/> to an <see cref="StandardNote"/> instance.
    /// </summary>
    public class NoteEntityToStandardNoteConverter : ITypeConverter<NoteEntity, StandardNote>
    {
        /// <summary>Performs conversion of a <see cref="NoteEntity"/> to an <see cref="StandardNote"/> instance.</summary>
        /// <param name="source">An instance of <see cref="StandardNote"/>.</param>
        /// <param name="destination">An instance of <see cref="StandardNote"/>.</param>
        /// <param name="context">Resolution context</param>
        /// <returns>An instance of <see cref="StandardNote"/> with values transferred over from source class.</returns>
        public StandardNote Convert(NoteEntity source, StandardNote destination, ResolutionContext context) =>
            new(
                source.Id, 
                source.Title, 
                source.Text,
                source.CreatedOn, 
                source.ModifiedOn);
    }
}