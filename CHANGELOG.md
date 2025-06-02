# Changelog

## Unreleased

- **Breaking change:** The Email Log entity, repository interface, and associated setting have all been removed. The
  benefit of having these items in the package was negligible. Any desired email logging will need to be independently
  implemented by the client application.

## [1.1.1] - 2025-06-02

- Fixed: Audit emails were formatted incorrectly for emails with no HTML body.

## [1.1.0] - 2025-01-28

- Target .NET 8 & 9.
- Fixed: Email sender was truncated more than required when saving to the repository.

## [1.0.0] - 2024-12-04

_Initial release._

[1.1.1]: https://github.com/gaepdit/email-service/releases/tag/v1.1.1

[1.1.0]: https://github.com/gaepdit/email-service/releases/tag/v1.1.0

[1.0.0]: https://github.com/gaepdit/email-service/releases/tag/v1.0.0
