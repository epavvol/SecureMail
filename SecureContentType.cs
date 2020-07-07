using System;
using System.Net.Mime;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// Secure email content type
    /// </summary>
    public class SecureContentType : ContentType
    {
        #region Constructors

        /// <summary>
        /// Creates secure email content type object instance
        /// </summary>
        public SecureContentType()
        {
        }

        /// <summary>
        /// Creates secure email content type object instance
        /// </summary>
        /// <param name="contentType">Content type</param>
        public SecureContentType(string contentType) : base(contentType)
        {
        }

        #endregion

        internal void GenerateBoundary() =>
            Boundary = $"--vnB=_{Guid.NewGuid().ToString().Replace('-', '_')}";
    }
}