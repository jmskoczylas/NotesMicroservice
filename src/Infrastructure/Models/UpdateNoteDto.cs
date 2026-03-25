namespace Infrastructure.Models
{
    /// <summary>
    /// Request model for updating an existing note.
    /// </summary>
    public class UpdateNoteDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the optimistic concurrency version.
        /// </summary>
        public int NoteVersion { get; set; }
    }
}
