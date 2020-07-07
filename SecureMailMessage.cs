using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using VNetDev.SecureMail.Extensions;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// Represents a secure email message that can be sent using the SmtpClient class
    /// </summary>
    public class SecureMailMessage : IDisposable
    {
        #region Properties

        private MailMessage MailMessage { get; }
        
        /// <summary>
        /// SecureMailAttachment collection
        /// </summary>
        public SecureAttachmentCollection Attachments { get; }
        
        /// <summary>
        /// Shows if message contains multipart data
        /// </summary>
        public bool IsMultiPart => !IsEncrypted && (Attachments.Count > 0 || IsSigned);
        
        /// <summary>
        /// Defines if mail message has to be signed before sending
        /// </summary>
        public bool IsSigned { get; set; }
        
        /// <summary>
        /// Defines if mail message has to be encrypted before sending
        /// </summary>
        public bool IsEncrypted { get; set; }
        
        /// <summary>
        /// Gets the attachment collection used to store alternate forms of the message body.
        /// </summary>
        public AlternateViewCollection AlternateViews => MailMessage.AlternateViews;

        /// <summary>
        /// Gets the secure address collection that contains the blind carbon copy (BCC) recipients for this email message.
        /// </summary>
        public MailAddressCollection Bcc => MailMessage.Bcc;

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public string Body
        {
            get => MailMessage.Body;
            set => MailMessage.Body = value;
        }

        /// <summary>
        /// Gets or sets the encoding used to encode the message body.
        /// </summary>
        public Encoding BodyEncoding
        {
            get => MailMessage.BodyEncoding;
            set => MailMessage.BodyEncoding = value;
        }

        /// <summary>
        /// Gets or sets the transfer encoding used to encode the message body.
        /// </summary>
        public TransferEncoding BodyTransferEncoding
        {
            get => MailMessage.BodyTransferEncoding;
            set => MailMessage.BodyTransferEncoding = value;
        }

        /// <summary>
        /// Gets the address collection that contains the carbon copy (CC) recipients for this email message.
        /// </summary>
        public MailAddressCollection CC => MailMessage.CC;

        /// <summary>
        /// Gets or sets the delivery notifications for this email message.
        /// </summary>
        public DeliveryNotificationOptions DeliveryNotificationOptions
        {
            get => MailMessage.DeliveryNotificationOptions;
            set => MailMessage.DeliveryNotificationOptions = value;
        }

        /// <summary>
        /// Gets or sets the from address for this email message.
        /// </summary>
        public SecureMailAddress? From
        {
            get => MailMessage.From.ToSecureMailAddress();
            set => MailMessage.From = value;
        }

        /// <summary>
        /// Gets the email headers that are transmitted with this email message.
        /// </summary>
        public NameValueCollection Headers => MailMessage.Headers;

        /// <summary>
        /// Gets or sets the encoding used for the user-defined custom headers for this email message.
        /// </summary>
        public Encoding HeadersEncoding
        {
            get => MailMessage.HeadersEncoding;
            set => MailMessage.HeadersEncoding = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mail message body is in HTML.
        /// </summary>
        public bool IsBodyHtml
        {
            get => MailMessage.IsBodyHtml;
            set => MailMessage.IsBodyHtml = value;
        }

        /// <summary>
        /// Gets or sets the priority of this email message.
        /// </summary>
        public MailPriority Priority
        {
            get => MailMessage.Priority;
            set => MailMessage.Priority = value;
        }

        /// <summary>
        /// Gets the list of addresses to reply to for the mail message.
        /// </summary>
        public MailAddressCollection ReplyToList => MailMessage.ReplyToList;

        /// <summary>
        /// Gets or sets the sender's address for this email message.
        /// </summary>
        public SecureMailAddress? Sender
        {
            get => MailMessage.Sender.ToSecureMailAddress();
            set => MailMessage.Sender = value;
        }

        /// <summary>
        /// Gets or sets the subject line for this email message.
        /// </summary>
        public string Subject
        {
            get => MailMessage.Subject;
            set => MailMessage.Subject = value;
        }

        /// <summary>
        /// Gets or sets the encoding used for the subject content for this email message.
        /// </summary>
        public Encoding SubjectEncoding
        {
            get => MailMessage.SubjectEncoding;
            set => MailMessage.SubjectEncoding = value;
        }

        /// <summary>
        /// Gets the address collection that contains the recipients of this email message.
        /// </summary>
        public MailAddressCollection To => MailMessage.To;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an empty instance of the SecureMailMessage class.
        /// </summary>
        public SecureMailMessage()
        {
            MailMessage = new MailMessage();
            Attachments = new SecureAttachmentCollection();
        }

        /// <summary>
        /// Initializes a new instance of the SecureMailMessage class by using
        /// the specified SecureMailAddress class objects.
        /// </summary>
        /// <param name="from">A SecureMailAddress that contains the address of the sender of the email message.</param>
        /// <param name="to">A SecureMailAddress that contains the address of the recipient of the email message.</param>
        public SecureMailMessage(MailAddress from, MailAddress to) : this()
        {
            From = from.ToSecureMailAddress();
            To.Add(to);
        }

        /// <summary>
        /// Initializes a new instance of the SecureMailMessage class.
        /// </summary>
        /// <param name="from">A String that contains the address of the sender of the email message.</param>
        /// <param name="to">A String that contains the address of the recipients of the email message.</param>
        /// <param name="subject">A String that contains the subject text.</param>
        /// <param name="body">A String that contains the message body.</param>
        public SecureMailMessage(string from, string to, string? subject = null, string? body = null)
            : this(new MailAddress(from), new MailAddress(to))
        {
            if (subject != null)
                Subject = subject;
            if (body != null)
                Body = body;
        }

        #endregion

        #region Methods

        private SecureMessageContent GetUnsignedContent()
        {
            var contentType = new SecureContentType
            {
                MediaType = IsBodyHtml ? "text/html" : "text/plain",
                CharSet = BodyEncoding.BodyName
            };
            var bodyEncoding = BodyEncoding ?? Encoding.ASCII;

            var secureContent = new SecureMessageContent(
                bodyEncoding.GetBytes(Body),
                contentType,
                TransferEncoding.Base64,
                IsMultiPart || IsEncrypted);

            if (Attachments.Count < 1)
                return secureContent;

            var bodyWithAttachments = new SecureContentType("multipart/mixed");
            bodyWithAttachments.GenerateBoundary();

            var sb = new StringBuilder()
                .AppendLineBreak()
                .AppendBoundary(bodyWithAttachments)
                .Append("Content-Type: ").Append(secureContent.ContentType).AppendLineBreak()
                .Append("Content-Transfer-Encoding: ").Append(secureContent.TransferEncoding)
                .AppendLineBreak().AppendLineBreak()
                .Append(Encoding.ASCII.GetString(secureContent.Body)).AppendLineBreak();

            foreach (var attachment in Attachments)
                sb.AppendBoundary(bodyWithAttachments)
                    .Append("Content-Type: ").Append(attachment.ContentType).AppendLineBreak()
                    .Append("Content-Transfer-Encoding: base64")
                    .AppendLineBreak().AppendLineBreak()
                    .Append(attachment.RawBytes.ToBase64String())
                    .AppendLineBreak().AppendLineBreak();

            sb.AppendBoundary(bodyWithAttachments, true);

            return new SecureMessageContent(
                Encoding.ASCII.GetBytes(sb.ToString()),
                bodyWithAttachments,
                TransferEncoding.SevenBit,
                false);
        }

        private SecureMessageContent SignContent(SecureMessageContent unsignedContent)
        {
            if (From == null)
                throw new InvalidOperationException("Sender address not specified!");
            if (From.SigningCertificate == null)
                throw new InvalidOperationException("Signing certificate not specified.");

            var contentType = new SecureContentType(
                "multipart/signed; protocol=\"application/x-pkcs7-signature\"; micalg=SHA1;");
            contentType.GenerateBoundary();

            var unsignedMessage = new StringBuilder()
                .Append("Content-Type: ").Append(unsignedContent.ContentType).AppendLineBreak()
                .Append("Content-Transfer-Encoding: ").Append(unsignedContent.TransferEncoding)
                .AppendLineBreak().AppendLineBreak()
                .Append(Encoding.ASCII.GetString(unsignedContent.Body))
                .ToString();

            var signatureBytes = unsignedMessage
                .GetSignature(From.SigningCertificate, From.EncryptionCertificate);

            var signedMessage = new StringBuilder()
                .AppendBoundary(contentType)
                .Append(unsignedMessage).AppendLineBreak()
                .AppendBoundary(contentType)
                .Append("Content-Type: application/x-pkcs7-signature;").AppendLineBreak()
                .Append("Content-Transfer-Encoding: base64").AppendLineBreak()
                .Append("Content-Disposition: attachment; filename=\"smime.p7s\"").AppendLineBreak()
                .Append("Content-Description: MIME message")
                .AppendLineBreak().AppendLineBreak()
                .Append(signatureBytes.ToBase64String())
                .AppendLineBreak().AppendLineBreak()
                .AppendBoundary(contentType, true)
                .ToString();

            return new SecureMessageContent(
                Encoding.ASCII.GetBytes(signedMessage),
                contentType,
                TransferEncoding.SevenBit,
                false);
        }

        private SecureMessageContent EncryptContent(SecureMessageContent unencryptedContent)
        {
            if (From == null)
                throw new InvalidOperationException("Sender address not specified!");
            
            var encryptionCertificates = new X509Certificate2Collection();
            
            if (From.EncryptionCertificate != null)
                encryptionCertificates.Add(From.EncryptionCertificate);

            foreach (var address in To.Concat(CC).Concat(Bcc))
                encryptionCertificates.Add(
                    address.ToSecureMailAddress()?.EncryptionCertificate
                    ?? throw new InvalidOperationException(
                        $"Email address '{address.Address}' does not have Encryption certificate specified."));

            var contentType = new SecureContentType(
                "application/x-pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\""
            );
            var unencryptedMessage = new StringBuilder()
                .Append("Content-Type: ").Append(unencryptedContent.ContentType).AppendLineBreak()
                .Append("Content-Transfer-Encoding: ").Append(unencryptedContent.TransferEncoding)
                .AppendLineBreak().AppendLineBreak()
                .Append(Encoding.ASCII.GetString(unencryptedContent.Body))
                .ToString();

            return new SecureMessageContent(
                unencryptedMessage.EncryptMessage(encryptionCertificates),
                contentType,
                TransferEncoding.Base64,
                false);
        }

        private MailMessage ToMailMessage()
        {
            var result = new MailMessage();
            if (From != null)
                result.From = From;
            if (Sender != null)
                result.Sender = Sender;
            foreach (var address in To)
                result.To.Add(address);
            foreach (var address in CC)
                result.CC.Add(address);
            foreach (var address in Bcc)
                result.Bcc.Add(address);
            result.DeliveryNotificationOptions = DeliveryNotificationOptions;
            foreach (string header in Headers)
                result.Headers.Add(header, Headers[header]);
            result.Priority = Priority;
            result.Subject = Subject;
            result.SubjectEncoding = SubjectEncoding;

            var content = GetUnsignedContent();
            if (IsSigned)
                content = SignContent(content);
            if (IsEncrypted)
                content = EncryptContent(content);

            var stream = new MemoryStream();

            if (IsMultiPart)
            {
                var mimeMessage = Encoding.ASCII.GetBytes(
                    "This is a multi-part message in MIME format.\r\n\r\n");
                stream.Write(mimeMessage, 0, mimeMessage.Length);
            }

            var encodedBody =
                content.TransferEncoding == TransferEncoding.SevenBit
                    ? Encoding.ASCII.GetBytes(
                        Regex.Replace(
                            Encoding.ASCII.GetString(content.Body),
                            "^\\.",
                            "..",
                            RegexOptions.Multiline))
                    : content.Body;

            stream.Write(encodedBody, 0, encodedBody.Length);
            stream.Position = 0;
            result.AlternateViews.Add(
                new AlternateView(
                    stream, content.ContentType)
                {
                    TransferEncoding = content.TransferEncoding
                });

            return result;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Helps to cast SecureMailMessage to System.Net.Mail.MailMessage.
        /// </summary>
        /// <param name="secureMailMessage">Input SecureMailMessage object</param>
        /// <returns>MailMessage object</returns>
        public static implicit operator MailMessage?(SecureMailMessage? secureMailMessage) =>
            secureMailMessage?.ToMailMessage();

        #endregion

        #region IDisposable implementation members

        /// <summary>
        /// Releases all resources used by the SecureMailMessage.
        /// </summary>
        public void Dispose() => MailMessage?.Dispose();

        #endregion
    }
}