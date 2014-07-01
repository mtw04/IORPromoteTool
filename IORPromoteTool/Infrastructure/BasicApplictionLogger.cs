// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Infrastructure
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// The basic application logger just logs everything quickly to the event log.
    /// </summary>
    public class BasicApplictionLogger : ApplicationLoggerBase, IApplicationLogger
    {
        /// <summary>
        /// Flag indicating logging is enabled.
        /// </summary>
        private bool isLoggingEnabled;

        /// <summary>
        /// The event log name.
        /// </summary>
        private string eventLogName;

        /// <summary>
        /// The event source.
        /// </summary>
        private string eventSource;

        /// <summary>
        /// The minimum event log type threshold. Any logging above this value will be ignored.
        /// </summary>
        private EventLogEntryType minimumEventLogThreshold;

        /// <summary>
        /// The event log to log to.
        /// </summary>
        private EventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicApplictionLogger"/> class.
        /// </summary>
        public BasicApplictionLogger()
            : this("Laserfiche Laserfiche.Answers", "LFAnswers", true, EventLogEntryType.Information)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicApplictionLogger" /> class.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <param name="eventLogName">Name of the event log.</param>
        /// <param name="isLoggingEnabled">if set to <c>true</c> [is logging enabled].</param>
        /// <param name="minimumEventLogThreshold">The minimum event log threshold.</param>
        public BasicApplictionLogger(string eventSource, string eventLogName, bool isLoggingEnabled, EventLogEntryType minimumEventLogThreshold)
        {
            this.eventSource = eventSource;
            this.isLoggingEnabled = isLoggingEnabled;
            this.eventLogName = eventLogName;
            this.minimumEventLogThreshold = minimumEventLogThreshold;
        }

        /// <summary>
        /// Gets or sets the minimum event log threshold.
        /// </summary>
        public EventLogEntryType MinimumEventLogThreshold
        {
            get { return this.minimumEventLogThreshold; }
            set { this.minimumEventLogThreshold = value; }
        }

        /// <summary>
        /// Gets or sets the name of the event log.
        /// </summary>
        public string EventLogName
        {
            get { return this.eventLogName; }
            set { this.eventLogName = value; }
        }

        /// <summary>
        ///   <para>
        /// Gets or sets a value indicating whether this instance has logging enabled.
        ///   </para>
        ///   <para>
        /// Call this property before doing any complicated exception handling.
        ///   </para>
        /// </summary>
        public bool IsLoggingEnabled
        {
            get { return this.isLoggingEnabled; }
            set { this.isLoggingEnabled = value; }
        }

        /// <summary>
        /// Typically indicates this exception is important enough to print to the event viewer.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="cacheException">If set to <c>true</c> cache the exception to prevent event log flooding.</param>
        /// <returns>
        /// True indicates that the exception should be rethrown.
        /// </returns>
        public bool HandleEvent(Exception error, LogPolicy policy, bool cacheException)
        {
            EventLogEntryType eventType = GetEventLogTypeFromPolicy(policy);
            if (!IsLoggingEnabled || eventType > this.minimumEventLogThreshold || error.Data.Contains(LogKeys.AlreadyLoggedKey))
            {
                return false;
            }
            
            this.WriteExceptionToEventViewer(error, eventType);
            return true;
        }

        /// <summary>
        /// Logs the specified message to a log file.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="formatArgs">The format args.</param>
        public void Log(string message, LogPolicy policy, params object[] formatArgs)
        {
            EventLogEntryType eventType = GetEventLogTypeFromPolicy(policy);
            if (!IsLoggingEnabled || eventType > this.minimumEventLogThreshold)
            {
                return;
            }

            this.WriteMessageToEventViewer(message, formatArgs, eventType);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public Exception Initialize()
        {
            try
            {
                this.eventLog = new EventLog(this.eventLogName);
                this.eventLog.Source = this.eventSource;

                if (!EventLog.SourceExists(this.eventSource))
                {
                    EventLog.CreateEventSource(this.eventSource, this.eventLogName);
                }

                return null;
            }
            catch (Exception ex)
            {
                this.IsLoggingEnabled = false;
                Debug.Assert(false, ex.Message, ex.ToString());
                try
                {
                    // Attempt to log this to Application log.
                    EventLog.WriteEntry(this.eventSource, string.Format("Failed to initialize event log. {0}", ex.ToString()), EventLogEntryType.Warning);
                }
                catch
                {
                }

                return ex;
            }
        }

        /// <summary>
        /// Creates the log entry from message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatArtgs">The format artgs.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <returns></returns>
        private void WriteMessageToEventViewer(string message, object[] formatArgs, EventLogEntryType eventType)
        {
            if (formatArgs != null && formatArgs.Length > 0)
            {
                try
                {
                    message = string.Format(message, formatArgs);
                }
                catch (FormatException ex)
                {
                    Debug.Assert(false, ex.Message, ex.ToString());
                }
            }

            try
            {
                this.eventLog.WriteEntry(message, eventType);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// Creates the log entry from exception.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="policy">The policy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void WriteExceptionToEventViewer(Exception error, EventLogEntryType eventType)
        {
            try
            {
                StringBuilder message = CreateExceptionMessage(error);

                this.eventLog.WriteEntry(message.ToString(), eventType);
                error.Data[LogKeys.AlreadyLoggedKey] = true;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// Gets the event log type from policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        private EventLogEntryType GetEventLogTypeFromPolicy(LogPolicy policy)
        {
            switch (policy)
            {
                case LogPolicy.GeneralError:
                case LogPolicy.RequestError:
                    return EventLogEntryType.Error;
                case LogPolicy.Warning:
                    return EventLogEntryType.Warning;
                case LogPolicy.Information:
                case LogPolicy.Trace:
                    return EventLogEntryType.Information;
                default:
                    Debug.Assert(false, "Unknown policy: " + policy.ToString());
                    return EventLogEntryType.Information;
            }
        }
    }
}
