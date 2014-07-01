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

    /// <summary>
    /// The application logger base is used to share methods between application loggers.
    /// </summary>
    public abstract class ApplicationLoggerBase
    {
        /// <summary>
        /// Includes the key in message.
        /// </summary>
        /// <param name="logKey">The log key.</param>
        /// <returns></returns>
        protected virtual bool IncludeKeyInMessage(LogKeys logKey)
        {
            switch (logKey)
            {
                case LogKeys.ExceptionHash:
                case LogKeys.AlreadyLoggedKey:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Includes the key in message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected virtual bool IncludeKeyInMessage(object key)
        {
            if (key is LogKeys)
            {
                return this.IncludeKeyInMessage((LogKeys)key);
            }

            return true;
        }

        /// <summary>
        /// Creates a string message from an exception.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The StringBuilder containing the message.</returns>
        protected virtual StringBuilder CreateExceptionMessage(Exception error)
        {
            StringBuilder message = new StringBuilder(error.ToString());
            IDictionary dictionary = error.Data;
            if (dictionary != null && dictionary.Count > 0)
            {
                List<object> keysToLog = new List<object>();
                foreach (object key in dictionary.Keys)
                {
                    if (IncludeKeyInMessage(key))
                    {
                        keysToLog.Add(key);
                    }
                }

                if(keysToLog.Count > 0)
                {
                    message.AppendLine();
                    message.AppendLine("Exception Data:");
                    foreach (object key in keysToLog)
                    {
                        object keyvalue = dictionary[key];
                        if (keyvalue != null)
                        {
                            message.AppendLine(string.Format("{0}: {1}", key, keyvalue));
                        }
                    }
                }
            }

            return message;
        }

        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatArgs">The format args.</param>
        /// <returns></returns>
        protected virtual string FormatMessage(string message, object[] formatArgs)
        {
            if (message != null && formatArgs != null && formatArgs.Length > 0)
            {
                try
                {
                    message = string.Format(message, formatArgs);
                }
                catch (FormatException ex)
                {
                    Debug.Assert(false, message, ex.ToString());
                }
            }

            return message;
        }

        /// <summary>
        /// Gets the key value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error">The error.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected virtual T GetKeyValue<T>(IDictionary additionalData, LogKeys key, T defaultValue)
        {
            if (HasKey(additionalData, key))
            {
                try
                {
                    return (T)additionalData[key];
                }
                catch(Exception ex)
                {
                    Debug.Assert(false, ex.Message, ex.ToString());
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Determines whether the specified error has key.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified error has key; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool HasKey(IDictionary additionalData, LogKeys key)
        {
            if (additionalData != null)
            {
                return additionalData.Contains(key);
            }

            return false;
        }

        /// <summary>
        /// Determines whether the exception has already been logged.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///   <c>true</c> if [has already been logged] [the specified error]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool HasAlreadyBeenLogged(Exception error)
        {
            return this.HasKey(error.Data, LogKeys.AlreadyLoggedKey);
        }
    }
}
