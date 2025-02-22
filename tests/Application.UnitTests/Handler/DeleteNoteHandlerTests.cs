using Application.Commands;
using Application.Handlers;
using FluentResults;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Handler
{
    public class DeleteNoteHandlerTests : Fixture
    {
        [Fact]
        public void NullRepository_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteNoteCommandHandler(null));
        }

        [Fact]
        public async Task ValidNote_Handle_ReturnsTrue()
        {
            this.NoteRepository
                .DeleteAsync(Arg.Any<int>())
                .Returns(Result.Ok(this.Note.Id));

            var sut = await new DeleteNoteCommandHandler(this.NoteRepository).Handle(
                new DeleteNoteCommand(this.Note.Id), CancellationToken.None);

            Assert.True(sut.IsSuccess);
            Assert.Equal(this.Note.Id, sut.ValueOrDefault);
        }
    }
}
