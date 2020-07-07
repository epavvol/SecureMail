# SecureMail
S/MIME Secure mail interface

#### Nuget package

https://www.nuget.org/packages/VNetDev.SecureMail/

#### Description

The ***SecureMailMessage*** could be used as regular ***System.Net.Mail.MailMessage***.

The difference is that ***SecureMailMessage*** requires ***SecureMailAddress*** objects that extends ***System.Net.Mail.MailAddress*** by adding 2 Certificate properties for signing and encryption purposes.

***SecureMailMessage*** object can be used with standard dotnet ***System.Net.Mail.SmtpClient*** as it can be casted to standard ***MailMessage*** type.

For specifying either message must be signed and/or encrypted ***SecureMailMessage*** has 2 bool properties **IsSigned** and **IsEncrypted** that can be defined.
