using Infrastructure.Models;
using System;
using Xunit;

namespace Application.UnitTests.Models
{
    public class NoteTests : Fixture
    {
        [Fact]
        public void ValidNote_Ctor_SuccessfullyCreated()
        {
            var sut = new NoteEntity(this.Note.Id, this.Note.Title, this.Note.Text, this.Note.CreatedOn,
                this.Note.ModifiedOn);

            Assert.Equal(this.Note.Id, sut.Id);
            Assert.Equal(this.Note.Title, sut.Title);
            Assert.Equal(this.Note.Text, sut.Text);
            Assert.Equal(this.Note.CreatedOn, sut.CreatedOn);
            Assert.Equal(this.Note.ModifiedOn, sut.ModifiedOn);
        }

        [Fact]
        public void InvalidId_Ctor_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NoteEntity(-1, this.Note.Title, this.Note.Text,
                this.Note.CreatedOn,
                this.Note.ModifiedOn));
        }

        [Theory] [InlineData(null)] [InlineData("")]
        public void InvalidTitle_Ctor_ThrowsArgumentNullException(string title)
        {
            Assert.Throws<ArgumentNullException>(() => new NoteEntity(this.Note.Id, title, this.Note.Text,
                this.Note.CreatedOn,
                this.Note.ModifiedOn));
        }
    }
}
