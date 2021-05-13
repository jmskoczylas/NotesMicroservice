using System.Diagnostics;

namespace Infrastructure.Tracing
{
    /// <summary>
    /// An extension class for open telemetry..
    /// </summary>
    public static class ActivityExtensions
    {
        /// <summary>
        /// Sets the the custom property indicating success.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public static void SetIsSuccess(this Activity activity, bool success)
        {
            activity?.SetCustomProperty("operationSuccessful", success);
        }

        /// <summary>
        /// Sets the the custom record count property.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="count">The record count.</param>
        public static void SetRecordCount(this Activity activity, int count)
        {
            activity?.SetCustomProperty("recordCount", count);
        }
    }
}
