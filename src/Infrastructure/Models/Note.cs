using System;

namespace Infrastructure.Models
{
    /// <inheritdoc />
    public class Note : Domain.Entities.Note
    {
        /// <inheritdoc />
        public Note(int id, string title, string notes, DateTime? createdOn, DateTime? modifiedOn) : base(id, title, notes, createdOn, modifiedOn)
        {
        }
    }
}
