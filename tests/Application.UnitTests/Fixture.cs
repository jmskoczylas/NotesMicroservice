using System;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using NSubstitute;

namespace Application.UnitTests
{
    public class Fixture
    {
        public INoteRepository NoteRepository { get; }
        public NoteRepositoryDto NoteRepositoryDto { get; }
        public IMapper Mapper { get; }
        public INote Note { get; }
        public CreateNoteDto CreateNoteDto { get; }
        public NoteDto NoteDto { get; }
        public UpdateNoteDto UpdateNoteDto { get; }

        public Fixture()
        {
            NoteRepository = Substitute.For<INoteRepository>();
            Note = new Note(474, "name", "the notes",
                DateTime.UtcNow, DateTime.UtcNow, 1, null);

            NoteDto = new NoteDto()
            {
                Id = Note.Id,
                Title = Note.Title,
                ModifiedOn = Note.ModifiedOn,
                CreatedOn = Note.CreatedOn,
                NoteVersion = Note.NoteVersion,
                DeletedOn = Note.DeletedOn,
                Text = Note.Text
            };

            CreateNoteDto = new CreateNoteDto()
            {
                Title = Note.Title,
                Text = Note.Text
            };

            UpdateNoteDto = new UpdateNoteDto()
            {
                Id = Note.Id,
                Title = Note.Title,
                Text = Note.Text,
                NoteVersion = Note.NoteVersion
            };

            NoteRepositoryDto = new NoteRepositoryDto()
            {
                Id = Note.Id,
                Title = Note.Title,
                Text = Note.Text,
                CreatedOn = Note.CreatedOn,
                ModifiedOn = Note.ModifiedOn,
                NoteVersion = Note.NoteVersion,
                DeletedOn = Note.DeletedOn
            };

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.ShouldMapMethod = _ => false;
                mc.AddProfile(new MapperProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            Mapper = mapper;
        }
    }
}
