using System.Net.Mail;

namespace VNetDev.SecureMail.Extensions
{
    internal static class MailAddressExtensions
    {
        internal static SecureMailAddress? ToSecureMailAddress(this MailAddress? mailAddress) =>
            mailAddress switch
            {
                null => null,
                SecureMailAddress secureMailAddress => secureMailAddress,
                _ => mailAddress.DisplayName == null
                    ? new SecureMailAddress(mailAddress.Address)
                    : new SecureMailAddress(mailAddress.Address, mailAddress.DisplayName)
            };
    }
}