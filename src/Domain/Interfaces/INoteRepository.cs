using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Represents the note repository for interacting with the data store..
    /// </summary>
    public interface INoteRepository : IRepository<INote>
    {
        /// <summary>
        /// Creates the new note.
        /// </summary>
        /// <param name="note">An instance of <see cref="INote"/> to create.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="INote"/> that was created.</returns>
        public new ValueTask<Result<INote>> CreateAsync(INote note);

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="note">An instance of <see cref="INote"/> to update.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="INote"/> that was updated.</returns>
        public new ValueTask<Result<INote>> UpdateAsync(INote note);

        /// <summary>
        /// Deletes the note.
        /// </summary>
        /// <param name="id">The identifier of the note to delete.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="INote"/> that was deleted.</returns>
        public new ValueTask<Result<int>> DeleteAsync(int id);

        /// <summary>
        /// Gets the note.
        /// </summary>
        /// <param name="id">The identifier of the note to get.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="INote"/> that was returned.</returns>
        public new ValueTask<Result<INote>> GetAsync(int id);

        /// <summary>
        /// Get all the notes.
        /// </summary>
        /// <param name="pageSize">The amount of records displayed on each page.</param>
        /// <param name="pageNumber">Relevant page based on the page size and amount of records in the database.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="INote"/> that was returned.</returns>
        public new Task<Result<IReadOnlyCollection<INote>>> GetAsync(int pageNumber, int pageSize);

    }
}
