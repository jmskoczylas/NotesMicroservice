namespace Infrastructure.Models
{
    /// <summary>
    /// Request model for creating a new note.
    /// </summary>
    public class CreateNoteDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}
