using Application.Handlers;
using Application.Requests;
using Domain.Interfaces;
using FluentResults;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests;
using Xunit;

namespace Application.Tests.Handler
{
    public class CreateNoteHandlerTests : Fixture
    {
        [Fact]
        public void NullMapper_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateNoteHandler(this.NoteRepository, null));
        }

        [Fact]
        public void NullRepository_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateNoteHandler(null, this.Mapper));
        }

        [Fact]
        public async Task ValidNote_Handle_ReturnsTrue()
        {
            this.NoteRepository
                .CreateAsync(Arg.Any<INote>())
                .Returns(Result.Ok(this.Note));

            var sut = await new CreateNoteHandler(this.NoteRepository, this.Mapper).Handle(
                new CreateNoteRequest(this.NoteDto), CancellationToken.None);

            Assert.True(sut.IsSuccess);
            Assert.Equal(this.NoteDto.Id, sut.ValueOrDefault.Id);
            Assert.Equal(this.NoteDto.Title, sut.ValueOrDefault.Title);
            Assert.Equal(this.NoteDto.Text, sut.ValueOrDefault.Text);
            Assert.Equal(this.NoteDto.ModifiedOn, sut.ValueOrDefault.ModifiedOn);
            Assert.Equal(this.NoteDto.CreatedOn, sut.ValueOrDefault.CreatedOn);
        }
    }
}
