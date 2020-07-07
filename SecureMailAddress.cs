using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// Secure mail address
    /// Extension for System.Net.Mail.MailAddress class
    /// Adds Encryption and Signing certificates
    /// </summary>
    public class SecureMailAddress : MailAddress
    {
        #region Properties

        /// <summary>
        /// Email encryption certificate
        /// </summary>
        public X509Certificate2? EncryptionCertificate { get; }

        /// <summary>
        /// Email signing certificate
        /// </summary>
        public X509Certificate2? SigningCertificate { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="useSameCertificateForSigning">Use same certificate for both email signing and encryption</param>
        public SecureMailAddress(string address,
            X509Certificate2? encryptionCertificate, bool useSameCertificateForSigning)
            : this(address,
                encryptionCertificate, useSameCertificateForSigning ? encryptionCertificate : null)
        {
        }

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="displayName">Email owner display name</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="useSameCertificateForSigning">Use same certificate for both email signing and encryption</param>
        public SecureMailAddress(string address, string displayName,
            X509Certificate2? encryptionCertificate, bool useSameCertificateForSigning)
            : this(address, displayName,
                encryptionCertificate, useSameCertificateForSigning ? encryptionCertificate : null)
        {
        }

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="displayName">Email owner display name</param>
        /// <param name="displayNameEncoding">Email owner display name encoding</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="useSameCertificateForSigning">Use same certificate for both email signing and encryption</param>
        public SecureMailAddress(string address, string displayName, Encoding displayNameEncoding,
            X509Certificate2? encryptionCertificate, bool useSameCertificateForSigning)
            : this(address, displayName, displayNameEncoding,
                encryptionCertificate, useSameCertificateForSigning ? encryptionCertificate : null)
        {
        }

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="signingCertificate">Certificate used for email signing</param>
        public SecureMailAddress(string address,
            X509Certificate2? encryptionCertificate = null, X509Certificate2? signingCertificate = null)
            : base(address)
        {
            EncryptionCertificate = encryptionCertificate;
            SigningCertificate = signingCertificate;
        }

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="displayName">Email owner display name</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="signingCertificate">Certificate used for email signing</param>
        public SecureMailAddress(string address, string displayName,
            X509Certificate2? encryptionCertificate = null, X509Certificate2? signingCertificate = null)
            : base(address, displayName)
        {
            EncryptionCertificate = encryptionCertificate;
            SigningCertificate = signingCertificate;
        }

        /// <summary>
        /// Creates SecureMailAddress object instance
        /// </summary>
        /// <param name="address">Email address</param>
        /// <param name="displayName">Email owner display name</param>
        /// <param name="displayNameEncoding">Email owner display name encoding</param>
        /// <param name="encryptionCertificate">Certificate used for email encryption</param>
        /// <param name="signingCertificate">Certificate used for email signing</param>
        public SecureMailAddress(string address, string displayName, Encoding displayNameEncoding,
            X509Certificate2? encryptionCertificate = null, X509Certificate2? signingCertificate = null)
            : base(address, displayName, displayNameEncoding)
        {
            EncryptionCertificate = encryptionCertificate;
            SigningCertificate = signingCertificate;
        }

        #endregion
    }
}