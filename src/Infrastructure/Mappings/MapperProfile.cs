using Application.Mappings.Converters;
using AutoMapper;
using Domain.Entities;
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
            this.CreateMap<StandardNote, NoteEntity>().ConvertUsing<StandardNoteToNoteEntityConverter>();
            this.CreateMap<NoteEntity, StandardNote>().ConvertUsing<NoteEntityToStandardNoteConverter>();
        }
    }
}
