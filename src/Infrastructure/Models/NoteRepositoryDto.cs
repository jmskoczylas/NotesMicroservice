using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    /// <summary>
    /// A dto for note object when retrieving information from data store.
    /// </summary>
    public class NoteRepositoryDto
    {
        /// <summary>
        /// Gets or sets the note title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the date and time the definition was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time this definition was last modified.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the note Id.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
