# Changelog

## Unreleased

- **Breaking change:** SMTP operations are now awaited allowing for appropriate capture of any exceptions.
- **Breaking change:** Email addresses are now validated when creating the email message. Invalid addresses will cause
  an `ArgumentException`.

## [1.2.0] - 2025-06-02

- **Breaking change:** The Email Log DB entity, repository interface, and associated setting have all been removed. The
  benefit of having these items in the package was negligible. Any desired email logging will need to be independently
  implemented by each client application. If your app relied on these items, you can find the originals here and copy
  them to your own codebase:

    - [EmailLog](https://github.com/gaepdit/email-service/blob/388fb6f29525a3930ba8a5e88ea83f192fdcc1ae/src/EmailService/EmailLogRepository/EmailLog.cs)
    - [IEmailLogRepository](https://github.com/gaepdit/email-service/blob/388fb6f29525a3930ba8a5e88ea83f192fdcc1ae/src/EmailService/EmailLogRepository/IEmailLogRepository.cs)

## [1.1.1] - 2025-06-02

- Fixed: Audit emails were formatted incorrectly for emails with no HTML body.

## [1.1.0] - 2025-01-28

- Target .NET 8 & 9.
- Fixed: Email sender was truncated more than required when saving to the repository.

## [1.0.0] - 2024-12-04

_Initial release._

[1.2.0]: https://github.com/gaepdit/email-service/releases/tag/v1.2.0

[1.1.1]: https://github.com/gaepdit/email-service/releases/tag/v1.1.1

[1.1.0]: https://github.com/gaepdit/email-service/releases/tag/v1.1.0

[1.0.0]: https://github.com/gaepdit/email-service/releases/tag/v1.0.0
