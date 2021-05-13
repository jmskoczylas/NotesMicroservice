using Application.Handlers;
using Application.Requests;
using Application.Tests;
using FluentResults;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Handler
{
    public class GetNoteHandlerTests : Fixture
    {
        [Fact]
        public void NullMapper_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new GetNoteHandler(this.NoteRepository, null));
        }

        [Fact]
        public void NullRepository_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new GetNoteHandler(null, this.Mapper));
        }

        [Fact]
        public async Task ValidNote_Handle_ReturnsTrue()
        {
            this.NoteRepository
                .GetAsync(Arg.Any<int>())
                .Returns(Result.Ok(this.Note));

            var sut = await new GetNoteHandler(this.NoteRepository, this.Mapper).Handle(
                new GetNoteRequest(this.NoteDto.Id), CancellationToken.None);

            Assert.True(sut.IsSuccess);
            Assert.Equal(this.NoteDto.Id, sut.ValueOrDefault.Id);
            Assert.Equal(this.NoteDto.Title, sut.ValueOrDefault.Title);
            Assert.Equal(this.NoteDto.Text, sut.ValueOrDefault.Text);
            Assert.Equal(this.NoteDto.ModifiedOn, sut.ValueOrDefault.ModifiedOn);
            Assert.Equal(this.NoteDto.CreatedOn, sut.ValueOrDefault.CreatedOn);
        }
    }
}
