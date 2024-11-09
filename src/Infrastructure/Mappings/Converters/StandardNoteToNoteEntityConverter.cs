using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Mappings.Converters
{
    /// <summary>
    /// Converts an instance of <see cref="StandardNote"/> to a <see cref="NoteEntity"/>.
    /// </summary>
    public class StandardNoteToNoteEntityConverter : ITypeConverter<StandardNote, NoteEntity>
    {
        /// <summary>Performs conversion of an <see cref="StandardNote"/> instance to a <see cref="NoteEntity"/>.</summary>
        /// <param name="source">An instance of <see cref="StandardNote"/>.</param>
        /// <param name="destination">An instance of <see cref="NoteEntity"/>.</param>
        /// <param name="context">Resolution context</param>
        /// <returns>An instance of <see cref="NoteEntity"/> with values transferred over from source class.</returns>
        public NoteEntity Convert(StandardNote source, NoteEntity destination, ResolutionContext context)
        {
            return new(source.Id, source.Title, source.Text, source.CreatedOn, source.ModifiedOn);
        }
    }
}
