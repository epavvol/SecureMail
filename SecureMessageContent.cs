using System.Net.Mime;
using System.Text;
using VNetDev.SecureMail.Extensions;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// Secure message content container
    /// </summary>
    public class SecureMessageContent
    {
        #region Properties

        /// <summary>
        /// Transfer encoding format
        /// </summary>
        public TransferEncoding TransferEncoding { get; }

        /// <summary>
        /// Secure content type
        /// </summary>
        public SecureContentType ContentType { get; }

        /// <summary>
        /// Secure message body byte array
        /// </summary>
        public byte[] Body { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates secure message content container
        /// </summary>
        /// <param name="body">Body byte array</param>
        /// <param name="contentType">Body content type</param>
        /// <param name="encoding">Transfer encoding</param>
        /// <param name="encodeBody">Encode body</param>
        public SecureMessageContent(byte[] body, SecureContentType contentType, TransferEncoding encoding,
            bool encodeBody)
        {
            Body = encodeBody && encoding == TransferEncoding.Base64
                ? Encoding.UTF8.GetBytes(body.ToBase64String())
                : body;

            TransferEncoding = encoding;
            ContentType = contentType;
        }

        #endregion
    }
}