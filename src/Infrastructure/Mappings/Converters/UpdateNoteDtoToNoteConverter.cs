using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Mappings.Converters
{
    /// <summary>
    /// Converts an update request into a note entity.
    /// </summary>
    public class UpdateNoteDtoToNoteConverter : ITypeConverter<UpdateNoteDto, INote>
    {
        /// <inheritdoc />
        public INote Convert(UpdateNoteDto source, INote destination, ResolutionContext context) =>
            new Note(source.Id, source.Title, source.Text, null, null, source.NoteVersion, null);
    }
}
