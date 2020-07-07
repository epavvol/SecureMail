using System.Text;

namespace VNetDev.SecureMail.Extensions
{
    internal static class StringBuilderExtensions
    {
        private const string LineBreak = "\r\n";
        
        internal static StringBuilder AppendLineBreak(this StringBuilder sb) => sb.Append(LineBreak);

        internal static StringBuilder AppendBoundary(
            this StringBuilder sb, SecureContentType contentType, bool appendDashes = false) =>
            sb.Append("--").Append(contentType.Boundary).Append(appendDashes ? "--" : "").AppendLineBreak();
    }
}