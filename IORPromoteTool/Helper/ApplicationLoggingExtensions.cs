// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using IORPromoteTool.Infrastructure;

    /// <summary>
    /// Extension methods and singleton handler for application logging.
    /// </summary>
    public static class ApplicationLoggingExtensions
    {
        /// <summary>
        /// The singleton logger.
        /// </summary>
        private static IApplicationLogger singletonLogger;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static IApplicationLogger Logger
        {
            get
            {
                if (singletonLogger != null)
                {
                    return singletonLogger;
                }

                return GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IApplicationLogger)) as IApplicationLogger;
            }
        }

        /// <summary>
        /// Sets the logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public static void SetLogger(IApplicationLogger logger)
        {
            singletonLogger = logger;
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exceptionData">The exception data. Should be name, value parameters. Ex: "User", this.User, "Data", this.Data.</param>
        public static void HandleError(this Exception error, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                logger.HandleEvent(error, LogPolicy.GeneralError, false);
            }
        }

        /// <summary>
        /// Handles the request exception.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <param name="exceptionData">The exception data.</param>
        public static void HandleRequestException(this Exception error, HttpRequest message, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                AddDataToError(error, message);
                logger.HandleEvent(error, LogPolicy.RequestError, false);
            }
        }

        /// <summary>
        /// Handles the request exception.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <param name="exceptionData">The exception data.</param>
        public static void HandleRequestException(this Exception error, HttpRequestBase message, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                AddDataToError(error, message);
                logger.HandleEvent(error, LogPolicy.RequestError, false);
            }
        }

        /// <summary>
        /// Handles the warning.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exceptionData">The exception data.</param>
        public static void HandleWarning(this Exception error, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                logger.HandleEvent(error, LogPolicy.Warning, false);
            }
        }

        /// <summary>
        /// Handles the warning.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exceptionData">The exception data.</param>
        public static void HandleInfoError(this Exception error, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                logger.HandleEvent(error, LogPolicy.Information, false);
            }
        }

        /// <summary>
        /// Handles the warning.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exceptionData">The exception data.</param>
        public static void HandleTraceError(this Exception error, params object[] exceptionData)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                AddDataToError(error, exceptionData);
                logger.HandleEvent(error, LogPolicy.Trace, false);
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatArgs">The format args.</param>
        public static void LogMessage(string message, params object[] formatArgs)
        {
            IApplicationLogger logger = Logger;
            if (logger != null)
            {
                logger.Log(message, LogPolicy.Information, formatArgs);
            }
        }

        /// <summary>
        /// Adds the data to error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        private static void AddDataToError(Exception error, HttpRequestBase message)
        {
            if (message == null)
            {
                return;
            }

            try
            {
                string user = string.Empty;
                if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null)
                {
                    user = Thread.CurrentPrincipal.Identity.Name;
                }
                string key = string.Format("{0};{1};{2};{3}", error.Message, message.RawUrl, message.HttpMethod, user);
                error.Data[LogKeys.Url] = message.RawUrl;
                error.Data[LogKeys.Method] = message.HttpMethod;
                error.Data[LogKeys.User] = user;
                error.Data[LogKeys.Headers] = message.Headers.ToString();
                error.Data[LogKeys.ExceptionHash] = key;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// Adds the data to error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        private static void AddDataToError(Exception error, HttpRequest message)
        {
            if (message == null)
            {
                return;
            }

            try
            {
                string user = string.Empty;
                if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null)
                {
                    user = Thread.CurrentPrincipal.Identity.Name;
                }
                string key = string.Format("{0};{1};{2};{3}", error.Message, message.RawUrl, message.HttpMethod, user);
                error.Data[LogKeys.Url] = message.RawUrl;
                error.Data[LogKeys.Method] = message.HttpMethod;
                error.Data[LogKeys.User] = user;
                error.Data[LogKeys.Headers] = message.Headers.ToString();
                error.Data[LogKeys.ExceptionHash] = key;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// Adds the data to error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exceptionData">The exception data.</param>
        private static void AddDataToError(Exception error, object[] exceptionData)
        {
            if (exceptionData != null)
            {
                try
                {
                    for (int i = 0; (i + 1) < exceptionData.Length; i += 2)
                    {
                        object key = exceptionData[i];
                        object value = exceptionData[i + 1];
                        error.Data[key] = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message, ex.ToString());
                }
            }
        }
    }
}
