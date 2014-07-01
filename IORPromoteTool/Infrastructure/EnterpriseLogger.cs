// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    /// <summary>
    /// The enterprise logger is a simple implementation of Enterprise Library logging.
    /// </summary>
    public class EnterpriseLogger : ApplicationLoggerBase, IApplicationLogger
    {
        /// <summary>
        /// Flag indicating if logging is enabled.
        /// </summary>
        private bool isLoggingEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseLogger"/> class.
        /// </summary>
        public EnterpriseLogger()
        {
        }

        /// <summary>
        ///   <para>
        /// Gets a value indicating whether this instance has logging enabled.
        ///   </para>
        ///   <para>
        /// Call this property before doing any complicated exception handling.
        ///   </para>
        /// </summary>
        public bool IsLoggingEnabled
        {
            get 
            {
                return this.isLoggingEnabled && Logger.IsLoggingEnabled();
            }

            set
            {
                this.isLoggingEnabled = true;
            }
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <returns>
        /// The exception (if any) that caused initialization to fail.
        /// </returns>
        public Exception Initialize()
        {
            // EL initializes itself.
            return null;
        }

        /// <summary>
        /// Typically indicates this exception is important enough to print to the event viewer.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="cacheException">if set to <c>true</c> cache the exception to prevent error flooding. This is currently Not Yet Implemented.</param>
        /// <returns>
        /// True indicates that the exception was logged.
        /// </returns>
        public bool HandleEvent(Exception error, LogPolicy policy, bool cacheException)
        {
            if (this.HasAlreadyBeenLogged(error))
            {
                return true;
            }

            // Exception Caching has not been implemented yet.
            StringBuilder message = this.CreateExceptionMessage(error);
            if (this.WriteLogEntry(message.ToString(), policy, error.Data))
            {
                error.Data[LogKeys.AlreadyLoggedKey] = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs the specified message to a log file.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="formatArgs">The format args.</param>
        public void Log(string message, LogPolicy policy, params object[] formatArgs)
        {
            message = this.FormatMessage(message, formatArgs);
            this.WriteLogEntry(message, policy, null);
        }

        /// <summary>
        /// Includes the key in message.
        /// </summary>
        /// <param name="logKey">The log key.</param>
        /// <returns></returns>
        protected override bool IncludeKeyInMessage(LogKeys logKey)
        {
            // These keys are logged by Enterprise Library specially.
            switch (logKey)
            {
                case LogKeys.EventID:
                case LogKeys.Priority:
                case LogKeys.Title:
                    return false;
            }

            return base.IncludeKeyInMessage(logKey);
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="additionalData">The additional data.</param>
        /// <returns>True if the message was logged.</returns>
        private bool WriteLogEntry(string message, LogPolicy policy, IDictionary additionalData)
        {
            try
            {
                string category = GetCategoryFromPolicy(policy);
                TraceEventType eventType = GetEventTypeFromPolicy(policy);
                string title = GetKeyValue(additionalData, LogKeys.Title, string.Empty);
                int priority = GetKeyValue(additionalData, LogKeys.Priority, 0);
                int eventId = GetKeyValue(additionalData, LogKeys.EventID, 100);
                Logger.Write(message, category, priority, eventId, eventType, title);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, message, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets the event type from policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        private TraceEventType GetEventTypeFromPolicy(LogPolicy policy)
        {
            switch (policy)
            {
                case LogPolicy.GeneralError:
                    return TraceEventType.Critical;
                case LogPolicy.RequestError:
                    return TraceEventType.Error;
                case LogPolicy.Warning:
                    return TraceEventType.Warning;
                case LogPolicy.Information:
                    return TraceEventType.Information;
                case LogPolicy.Trace:
                    return TraceEventType.Verbose;
                default:
                    Debug.Assert(false, "Unknown policy: " + policy.ToString());
                    return TraceEventType.Information;
            }
        }

        /// <summary>
        /// Gets the category from policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        private string GetCategoryFromPolicy(LogPolicy policy)
        {
            return "General";
        }
    }
}
