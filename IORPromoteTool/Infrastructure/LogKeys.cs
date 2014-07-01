// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.
namespace IORPromoteTool.Infrastructure
{
    using System;

    /// <summary>
    /// Log keys are used to add additional data to an exception.
    /// </summary>
    [Serializable]
    public enum LogKeys
    {
        /// <summary>
        /// Email - the log key for email addresses.
        /// </summary>
        Email,

        /// <summary>
        /// Url - the log key for urls.
        /// </summary>
        Url,

        /// <summary>
        /// Method - the log key for HTTP Method.
        /// </summary>
        Method,

        /// <summary>
        /// User - the log key for HTTP Users.
        /// </summary>
        User,

        /// <summary>
        /// The headers for an HTTP message.
        /// </summary>
        Headers,

        /// <summary>
        /// ExceptionHash - special attribute for caching exceptions.
        /// </summary>
        ExceptionHash,

        /// <summary>
        /// The already logged key indicates this exception has been logged already and to not log it again.
        /// </summary>
        AlreadyLoggedKey,

        /// <summary>
        /// Special attribute indicating the event ID.
        /// </summary>
        EventID,

        /// <summary>
        /// Special attribute indicating the priority.
        /// </summary>
        Priority,

        /// <summary>
        /// Special attribute indiciting the title.
        /// </summary>
        Title,

        /// <summary>
        /// The subject of an e-mail.
        /// </summary>
        Subject,

        /// <summary>
        /// A search catalog id.
        /// </summary>
        CatalogID,

        /// <summary>
        /// A search catalog host.
        /// </summary>
        CatalogHost,

        /// <summary>
        /// The ID of an object.
        /// </summary>
        ID,

        /// <summary>
        /// The crop value of an image.
        /// </summary>
        Crop,

        /// <summary>
        /// The size value of an image.
        /// </summary>
        Size,

        /// <summary>
        /// The name of an object.
        /// </summary>
        Name,

        /// <summary>
        /// The post ID.
        /// </summary>
        PostID,

        /// <summary>
        /// The post title
        /// </summary>
        PostTitle,

        /// <summary>
        /// The category of an object.
        /// </summary>
        Category,
    }
}
