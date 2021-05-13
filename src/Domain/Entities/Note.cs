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
        /// Initializes a new instance of the <see cref="Note"/> class.
        /// </summary>
        /// <param name="id">The Id of the note.</param>
        /// <param name="title">The title of the note.</param>
        /// <param name="notes">The note notes.</param>
        /// <param name="createdOn">The <see cref="DateTime"/> when the note was created.</param>
        /// <param name="modifiedOn">The <see cref="DateTime"/> when the note was last modified.</param>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        /// <exception cref="ArgumentNullException">title</exception>
        protected Note(int id, string title, string notes, DateTime? createdOn, DateTime? modifiedOn)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            Text = notes;
            Id = id;
            Title = title;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
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
    }
}
