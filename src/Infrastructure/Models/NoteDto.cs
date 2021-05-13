using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    /// <summary>
    /// A dto for the note object.
    /// </summary>
    public class NoteDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets the date and time the entity was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets the date and time the entity was last modified.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
