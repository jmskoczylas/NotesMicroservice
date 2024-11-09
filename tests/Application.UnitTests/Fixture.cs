using Application.DTOs;
using AutoMapper;
using Domain.Entities;
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
        public NoteEntity NoteEntity { get; }
        public IMapper Mapper { get; }
        public INote Note { get; }
        public NoteDto NoteDto { get; }

        public Fixture()
        {
            this.NoteRepository = Substitute.For<INoteRepository>();
            this.Note = new StandardNote(474, "name", "the notes",
                DateTime.UtcNow, DateTime.UtcNow);

            this.NoteDto = new NoteDto()
            {
                Id = this.Note.Id,
                Title = this.Note.Title,
                ModifiedOn = this.Note.ModifiedOn,
                CreatedOn = this.Note.CreatedOn,
                Text = this.Note.Text
            };

            this.NoteEntity = new NoteEntity(this.Note.Id, this.Note.Title, this.Note.Text, this.Note.CreatedOn, this.Note.ModifiedOn);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            this.Mapper = mapper;
        }
    }
}
