using Application.Handlers;
using Application.Requests;
using Domain.Interfaces;
using FluentResults;
using Infrastructure.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Handler
{
    public class GetNotesHandlerTests : Fixture
    {
        [Fact]
        public void NullMapper_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new GetNotesHandler(this.NoteRepository, null));
        }

        [Fact]
        public void NullRepository_Ctor_CtorSuccessful()
        {
            Assert.Throws<ArgumentNullException>(() => new GetNotesHandler(null, this.Mapper));
        }

        [Fact]
        public async Task ValidNotes_Handle_ReturnsTrue()
        {
            IReadOnlyCollection<INote> notes = new List<INote>
            {
                this.Note,
                this.Note,
                this.Note,
                this.Note,
                this.Note,
                new NoteEntity(101, "last note", "some text", DateTime.UtcNow, DateTime.UtcNow)
            };

            IReadOnlyCollection<INote> page2 = notes.TakeLast(3).ToList();

            this.NoteRepository
                .GetAsync(Arg.Any<int>(), Arg.Any<int>())
                .Returns(Result.Ok(page2));

            var sut = await new GetNotesHandler(this.NoteRepository, this.Mapper).Handle(
                new GetNotesRequest(2, 3), CancellationToken.None);

            Assert.True(sut.IsSuccess);
            Assert.Equal(3, sut.ValueOrDefault.Count);
            Assert.Equal(notes?.LastOrDefault()?.Id, sut?.ValueOrDefault?.LastOrDefault()?.Id);
            Assert.Equal(notes?.LastOrDefault()?.Title, sut?.ValueOrDefault?.LastOrDefault()?.Title);
            Assert.Equal(notes?.LastOrDefault()?.Text, sut?.ValueOrDefault?.LastOrDefault()?.Text);
            Assert.Equal(notes?.LastOrDefault()?.ModifiedOn, sut?.ValueOrDefault?.LastOrDefault()?.ModifiedOn);
            Assert.Equal(notes?.LastOrDefault()?.CreatedOn, sut?.ValueOrDefault?.LastOrDefault()?.CreatedOn);
        }
    }
}
