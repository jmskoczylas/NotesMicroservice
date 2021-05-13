using Application.Handlers;
using Application.Requests;
using Application.Tests;
using Domain.Interfaces;
using FluentResults;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Handler
{
    public class UpdateNoteHandlerTests : Fixture
    {
        [Fact]
        public void NullMapper_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new UpdateNoteHandler(this.NoteRepository, null));
        }

        [Fact]
        public void NullRepository_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new UpdateNoteHandler(null, this.Mapper));
        }

        [Fact]
        public async Task ValidNote_Handle_ReturnsTrue()
        {
            this.NoteRepository
                .UpdateAsync(Arg.Any<INote>())
                .Returns(Result.Ok(this.Note));

            var sut = await new UpdateNoteHandler(this.NoteRepository, this.Mapper).Handle(
                new UpdateNoteRequest(this.NoteDto), CancellationToken.None);

            Assert.True(sut.IsSuccess);
            Assert.Equal(this.NoteDto.Id, sut.ValueOrDefault.Id);
            Assert.Equal(this.NoteDto.Title, sut.ValueOrDefault.Title);
            Assert.Equal(this.NoteDto.Text, sut.ValueOrDefault.Text);
            Assert.Equal(this.NoteDto.ModifiedOn, sut.ValueOrDefault.ModifiedOn);
            Assert.Equal(this.NoteDto.CreatedOn, sut.ValueOrDefault.CreatedOn);
        }
    }
}
