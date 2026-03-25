using System;

namespace Domain.Interfaces
{
    /// <summary>
    /// Represents a note entity.
    /// </summary>
    /// <seealso cref="int" />
    public interface INote : IEntity<int>, IAuditing
    {
        /// <summary>
        /// Gets the name of the record.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the optimistic concurrency version of the note.
        /// </summary>
        public int NoteVersion { get; }

        /// <summary>
        /// Gets the soft-delete timestamp, if note is deleted.
        /// </summary>
        public DateTime? DeletedOn { get; }
    }
}
