using Domain.Interfaces;
using System;

namespace Infrastructure.Models
{
    /// <inheritdoc />
    public class NoteEntity : INote
    {
        public int Id { get; private set; }

        public string Title { get; set; }

        public string Text { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public DateTime ModifiedOn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEntity"/> class with all properties.
        /// </summary>
        /// <param name="id">The ID of the note.</param>
        /// <param name="title">The title of the note.</param>
        /// <param name="text">The content of the note.</param>
        /// <param name="createdOn">The date the note was created.</param>
        /// <param name="modifiedOn">The date the note was last modified.</param>
        public NoteEntity(int id, string title, string text, DateTime createdOn, DateTime modifiedOn)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "ID must be non-negative.");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text), "Text cannot be null or empty.");

            Id = id;
            Title = title;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
            Text = text;
        }

    }
}
