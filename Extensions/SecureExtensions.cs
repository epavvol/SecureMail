using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VNetDev.SecureMail.Extensions
{
    internal static class SecureExtensions
    {
        internal static byte[] GetSignature(this string message, X509Certificate2 signingCertificate,
            X509Certificate2? encryptionCertificate = null)
        {
            var messageBytes = Encoding.ASCII.GetBytes(message);
            var signedCms = new SignedCms(new ContentInfo(messageBytes), true);
            var cmsSigner = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, signingCertificate);
            cmsSigner.IncludeOption = X509IncludeOption.WholeChain;
            if (encryptionCertificate != null)
                cmsSigner.Certificates.Add(encryptionCertificate);
            cmsSigner.SignedAttributes.Add(new Pkcs9SigningTime());
            signedCms.ComputeSignature(cmsSigner, false);
            return signedCms.Encode();
        }

        internal static byte[] EncryptMessage(this string message, X509Certificate2Collection encryptionCertificates)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var envelopedCms = new EnvelopedCms(new ContentInfo(messageBytes));
            var recipients =
                new CmsRecipientCollection(SubjectIdentifierType.IssuerAndSerialNumber, encryptionCertificates);
            envelopedCms.Encrypt(recipients);
            return envelopedCms.Encode();
        }
    }
}