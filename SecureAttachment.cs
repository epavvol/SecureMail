using System;
using System.IO;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// SecureMailMessage attachment
    /// </summary>
    public class SecureAttachment
    {
        #region Properties

        internal byte[] RawBytes { get; }

        /// <summary>
        /// Attachment content type
        /// </summary>
        public SecureContentType ContentType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="fileName">File path</param>
        /// <param name="mediaType">Attachment media type</param>
        public SecureAttachment(string fileName, string? mediaType = null)
            : this(
                File.ReadAllBytes(fileName),
                mediaType == null
                    ? new SecureContentType()
                    : new SecureContentType(mediaType),
                Path.GetFileName(fileName))
        {
        }

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="fileName">File path</param>
        /// <param name="contentType">Attachment content type</param>
        public SecureAttachment(string fileName, SecureContentType contentType)
            : this(File.ReadAllBytes(fileName), contentType, Path.GetFileName(fileName))
        {
        }

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="contentStream">Attachment content stream</param>
        /// <param name="name">Attachment name</param>
        /// <param name="mediaType">Attachment media type</param>
        public SecureAttachment(Stream contentStream, string name, string? mediaType = null)
            : this(
                contentStream,
                mediaType == null
                    ? new SecureContentType()
                    : new SecureContentType(mediaType),
                name)
        {
        }

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="contentStream">Attachment content stream</param>
        /// <param name="contentType">Attachment content type</param>
        /// <param name="name">Attachment name</param>
        /// <exception cref="ArgumentNullException">If contentStream is null.</exception>
        public SecureAttachment(Stream contentStream, SecureContentType contentType, string? name = null)
        {
            if (contentStream == null)
                throw new ArgumentNullException(nameof(contentStream));

            contentStream.Position = 0;
            var reader = new BinaryReader(contentStream);

            RawBytes = reader.ReadBytes((int) contentStream.Length);
            ContentType = contentType;

            if (name != null)
                ContentType.Name = name;
        }

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="contentBytes">Attachment byte array</param>
        /// <param name="name">Attachment name</param>
        /// <param name="mediaType">Attachment media type</param>
        public SecureAttachment(byte[] contentBytes, string name, string? mediaType = null)
            : this(
                contentBytes,
                mediaType == null
                    ? new SecureContentType {Name = name}
                    : new SecureContentType(mediaType) {Name = name})
        {
        }

        /// <summary>
        /// Creates secure attachment object
        /// </summary>
        /// <param name="contentBytes">Attachment byte array</param>
        /// <param name="contentType">Attachment content type</param>
        /// <param name="name">Attachment name</param>
        public SecureAttachment(byte[] contentBytes, SecureContentType contentType, string? name = null)
        {
            RawBytes = contentBytes ?? throw new ArgumentNullException(nameof(contentBytes));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            if (name != null)
                ContentType.Name = name;
        }

        #endregion
    }
}