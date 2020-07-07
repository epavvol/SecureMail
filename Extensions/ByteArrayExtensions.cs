namespace VNetDev.SecureMail.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static string ToBase64String(this byte[] bytes) => System.Convert
            .ToBase64String(bytes, System.Base64FormattingOptions.InsertLineBreaks);
    }
}