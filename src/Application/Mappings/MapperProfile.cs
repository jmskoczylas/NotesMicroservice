using Application.DTOs;
using Application.Mappings.Converters;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
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
            this.CreateMap<StandardNote, NoteDto>().ConvertUsing<StandardNoteToNoteDtoConverter>();
        }
    }
}
