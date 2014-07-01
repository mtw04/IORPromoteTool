// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;

    /// <summary>
    /// Interface to be implemented by the application logger.
    /// </summary>
    public interface IApplicationLogger
    {
        /// <summary>
        /// <para>
        /// Gets a value indicating whether this instance has logging enabled.
        /// </para>
        /// <para>
        /// Call this property before doing any complicated exception handling.
        /// </para>
        /// </summary>
        bool IsLoggingEnabled { get; }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <returns>The exception (if any) that caused initialization to fail.</returns>
        Exception Initialize();

        /// <summary>
        /// Typically indicates this exception is important enough to print to the event viewer.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="cacheException">if set to <c>true</c> [cache exception].</param>
        /// <returns>
        /// True indicates that the exception was logged.
        /// </returns>
        bool HandleEvent(Exception error, LogPolicy policy, bool cacheException);

        /// <summary>
        /// Logs the specified message to a log file or event viewer (depending on policy).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="formatArgs">The format args.</param>
        void Log(string message, LogPolicy policy, params object[] formatArgs);
    }
}
