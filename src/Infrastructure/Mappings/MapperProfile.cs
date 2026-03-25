using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Mappings.Converters;
using Infrastructure.Models;

namespace Infrastructure.Mappings
{
    /// <summary>
    /// Used to configure how automapper will map note objects.
    /// </summary>
    /// <seealso cref="Profile" />
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// </summary>
        public MapperProfile()
        {
            ShouldMapMethod = _ => false;

            CreateMap<INote, NoteDto>().ConvertUsing<NoteToNoteDtoConverter>();
            CreateMap<NoteDto, INote>().ConvertUsing<NoteDtoToNoteConverter>();
            CreateMap<NoteRepositoryDto, INote>()
                .ConstructUsing(x => new Note(x.Id, x.Title, x.Text, x.CreatedOn, x.ModifiedOn, x.NoteVersion, x.DeletedOn));
        }
    }
}
