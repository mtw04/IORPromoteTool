// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Infrastructure
{
    /// <summary>
    /// The error policy is used to indicate how to log the error.
    /// </summary>
    public enum LogPolicy
    {
        /// <summary>
        /// A general error - usually indicates a critical failure in the program.
        /// </summary>
        GeneralError,

        /// <summary>
        /// A request error - usually indicates an error caused by a user interaction and is not critical to the program.
        /// </summary>
        RequestError,

        /// <summary>
        /// A warning - usually indicates information that may be important but is not yet an error.
        /// </summary>
        Warning,

        /// <summary>
        /// Information - information that is worth showing to an administrator but doesn't require their attention.
        /// </summary>
        Information,

        /// <summary>
        /// Trace - indicates diagnostic information and should usually not be displayed.
        /// </summary>
        Trace
    }
}
