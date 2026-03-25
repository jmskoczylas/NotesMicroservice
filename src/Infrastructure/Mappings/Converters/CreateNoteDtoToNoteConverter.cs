using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Mappings.Converters
{
    /// <summary>
    /// Converts a create request into a note entity.
    /// </summary>
    public class CreateNoteDtoToNoteConverter : ITypeConverter<CreateNoteDto, INote>
    {
        /// <inheritdoc />
        public INote Convert(CreateNoteDto source, INote destination, ResolutionContext context) =>
            new Note(0, source.Title, source.Text, null, null, 0, null);
    }
}
