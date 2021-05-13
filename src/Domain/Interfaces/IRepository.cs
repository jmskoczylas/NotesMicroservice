using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Represents the generic repository for interacting with the data store.
    /// </summary>
    public interface IRepository<T> where T : IEntity<int>
    {
        /// <summary>
        /// Get all the items.
        /// </summary>
        /// <param name="pageSize">The amount of records displayed on each page.</param>
        /// <param name="pageNumber">Relevant page based on the page size and amount of records in the database.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and a collection of <see cref="T"/> that were returned.</returns>
        Task<Result<IReadOnlyCollection<T>>> GetAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Gets the note.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T"/> to get.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="T"/> that was returned.</returns>
        ValueTask<Result<T>> GetAsync(int id);

        /// <summary>
        /// Creates the new item.
        /// </summary>
        /// <param name="item">An instance of <see cref="T"/> to create.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="T"/> that was created.</returns>
        ValueTask<Result<T>> CreateAsync(T item);

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="item">An instance of <see cref="T"/> to update.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and <see cref="T"/> that was updated.</returns>
        ValueTask<Result<INote>> UpdateAsync(T item);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier of the item to delete.</param>
        /// <returns>A <see cref="Result{T}"/> indicating whether operation was successful and id of the <see cref="T"/> that was deleted.</returns>
        ValueTask<Result<int>> DeleteAsync(int id);
    }
}
