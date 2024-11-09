using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using NSubstitute;
using System;

namespace Application.UnitTests
{
    public class Fixture
    {
        public INoteRepository NoteRepository { get; }
        public NoteRepositoryDto NoteRepositoryDto { get; }
        public IMapper Mapper { get; }
        public INote Note { get; }
        public NoteDto NoteDto { get; }

        public Fixture()
        {
            this.NoteRepository = Substitute.For<INoteRepository>();
            this.Note = new Note(474, "name", "the notes",
                DateTime.UtcNow, DateTime.UtcNow);

            this.NoteDto = new NoteDto()
            {
                Id = this.Note.Id,
                Title = this.Note.Title,
                ModifiedOn = this.Note.ModifiedOn,
                CreatedOn = this.Note.CreatedOn,
                Text = this.Note.Text
            };

            this.NoteRepositoryDto = new NoteRepositoryDto()
            {
                Id = this.Note.Id,
                Title = this.Note.Title,
                Text = this.Note.Text,
                CreatedOn = this.Note.CreatedOn,
                ModifiedOn = this.Note.ModifiedOn,
            };

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            this.Mapper = mapper;
        }
    }
}
