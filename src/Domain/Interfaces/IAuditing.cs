using System;

namespace Domain.Interfaces
{
    public interface IAuditing
    {
        /// <summary>
        /// Gets the date and time the entity was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// Gets the date and time the entity was last modified.
        /// </summary>
        public DateTime ModifiedOn { get; }
    }
}
