using Application.Tests;
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
            var sut = new Note(this.Note.Id, this.Note.Title, this.Note.Text, this.Note.CreatedOn,
                this.Note.ModifiedOn, this.Note.NoteVersion, this.Note.DeletedOn);

            Assert.Equal(this.Note.Id, sut.Id);
            Assert.Equal(this.Note.Title, sut.Title);
            Assert.Equal(this.Note.Text, sut.Text);
            Assert.Equal(this.Note.CreatedOn, sut.CreatedOn);
            Assert.Equal(this.Note.ModifiedOn, sut.ModifiedOn);
            Assert.Equal(this.Note.NoteVersion, sut.NoteVersion);
            Assert.Equal(this.Note.DeletedOn, sut.DeletedOn);
        }

        [Fact]
        public void InvalidId_Ctor_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Note(-1, this.Note.Title, this.Note.Text,
                this.Note.CreatedOn,
                this.Note.ModifiedOn, this.Note.NoteVersion, this.Note.DeletedOn));
        }

        [Theory] [InlineData(null)] [InlineData("")]
        public void InvalidTitle_Ctor_ThrowsArgumentNullException(string title)
        {
            Assert.Throws<ArgumentNullException>(() => new Note(this.Note.Id, title, this.Note.Text,
                this.Note.CreatedOn,
                this.Note.ModifiedOn, this.Note.NoteVersion, this.Note.DeletedOn));
        }
    }
}
