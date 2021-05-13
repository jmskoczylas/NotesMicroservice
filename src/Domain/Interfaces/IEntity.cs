namespace Domain.Interfaces
{
    /// <summary>
    /// Represents a generic entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<out T>
    {
        /// <summary>
        /// Gets the Id of the record.
        /// </summary>
        public T Id { get; }
    }
}
