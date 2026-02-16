using System.Diagnostics;

namespace Infrastructure.Tracing
{
    /// <summary>
    /// Extension methods for adding custom tracing metadata to <see cref="Activity"/> instances.
    /// </summary>
    public static class ActivityExtensions
    {
        /// <summary>
        /// Sets a custom property indicating whether an operation succeeded.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="success"><see langword="true"/> when the operation succeeded; otherwise, <see langword="false"/>.</param>
        public static void SetIsSuccess(this Activity activity, bool success)
        {
            activity?.SetCustomProperty("operationSuccessful", success);
        }

        /// <summary>
        /// Sets a custom property with the number of records processed.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="count">The record count.</param>
        public static void SetRecordCount(this Activity activity, int count)
        {
            activity?.SetCustomProperty("recordCount", count);
        }
    }
}
