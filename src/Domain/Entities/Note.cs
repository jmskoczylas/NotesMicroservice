using Domain.Interfaces;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a note object.
    /// </summary>
    public abstract class Note : INote
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The ID of the note.</param>
        /// <param name="title">The title of the note.</param>
        /// <param name="notes">The note notes.</param>
        /// <param name="createdOn">The <see cref="DateTime"/> when the note was created.</param>
        /// <param name="modifiedOn">The <see cref="DateTime"/> when the note was last modified.</param>
        /// <param name="noteVersion">The optimistic concurrency version.</param>
        /// <param name="deletedOn">The <see cref="DateTime"/> when the note was soft-deleted.</param>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        /// <exception cref="ArgumentNullException">title</exception>
        protected Note(int id, string title, string notes, DateTime? createdOn, DateTime? modifiedOn, int noteVersion, DateTime? deletedOn)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);
            ArgumentOutOfRangeException.ThrowIfNegative(noteVersion);

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            Text = notes;
            Id = id;
            Title = title;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
            NoteVersion = noteVersion;
            DeletedOn = deletedOn;
        }

        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public string Text { get; }

        /// <inheritdoc />
        public DateTime? CreatedOn { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOn { get; }

        /// <inheritdoc />
        public int NoteVersion { get; }

        /// <inheritdoc />
        public DateTime? DeletedOn { get; }
    }
}
