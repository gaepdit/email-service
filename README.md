# Georgia EPD-IT Email Service Library

This library was created by Georgia EPD-IT to provide common email services for our web applications.

[![Georgia EPD-IT](https://raw.githubusercontent.com/gaepdit/gaepd-brand/main/blinkies/blinkies.cafe-gaepdit.gif)](https://github.com/gaepdit)
[![.NET Test](https://github.com/gaepdit/email-service/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/email-service/actions/workflows/dotnet.yml)
[![SonarCloud Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_email-service&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_email-service)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_email-service&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_email-service)

## Installation

[![Nuget](https://img.shields.io/nuget/v/GaEpd.EmailService)](https://www.nuget.org/packages/GaEpd.EmailService)

To install, search for "GaEpd.EmailService" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.EmailService`

## Usage

The `IEmailService` interface provides a single method, `SendEmailAsync(Message message)`, which sends an email
`Message`. Typical usage would look like this:

```csharp
// Create the email message.
var message = Message.Create(subject, recipientEmail, textBody, htmlBody, senderName, senderEmail, [ccRecipient]);

// Send the message. There is no need to await this call.
_ = emailService.SendEmailAsync(message, token);
```

## Configuration

The `AddEmailService()` extension method can be used to register the included implementation with your configuration
settings. The `DefaultEmailService` implementation uses [MailKit](https://github.com/jstedfast/MailKit) to send emails.

```csharp
builder.Services.AddEmailService(builder.Configuration);
```

The following configuration section should be added to your configuration file:

```json
{
  "EmailServiceSettings": {
    "EnableEmail": true,
    "SmtpHost": "localhost",
    "SmtpPort": 25,
    "SecureSocketOption": "Auto",
    "DefaultSenderName": "Default Sender",
    "DefaultSenderEmail": "Default.Sender@email.invalid",
    "EnableEmailAuditing": true,
    "AuditEmailRecipients": [
      "Audit.Recipient@email.invalid"
    ]
  }
}
```

* `EnableEmail`: Set to `true` to enable sending emails.
* `SmtpHost`: The SMTP server to use for sending emails. *(Only used if `EnableEmail` is `true`.)*
* `SmtpPort`: The port to use for the SMTP server. *(Only used if `EnableEmail` is `true`.)*
* `SecureSocketOption`: The MailKit `SecureSocketOption` to use for the SMTP server.  *(Only used if `EnableEmail` is
  `true`.)* Available options are `None`, `Auto`, `SslOnConnect`, `StartTls`, and `StartTlsWhenAvailable`. See
  the [MailKit documentation](https://github.com/jstedfast/MailKit/blob/5af38db5b64784e90aaa0c348987bf690044e063/MailKit/Security/SecureSocketOptions.cs)
  for a description of each option.
* `DefaultSenderName`: The default sender name *(optional, can also be set per message)*.
* `DefaultSenderEmail`: The default sender email address *(optional, can also be set per message)*.
* `EnableEmailAuditing`: Set to `true` to enable sending audit emails (useful for testing purposes).
* `AuditEmailRecipients`: A list of email addresses to send audit emails to. *(Only used if `EnableEmailAuditing` is
  `true`.)*

Note that `EnableEmail` and `EnableEmailAuditing` operate independently of each other. You can enable or disable each
feature individually.
