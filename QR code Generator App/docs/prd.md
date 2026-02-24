# Product Requirements Document (PRD)

## Goals and Background Context

### Goals
- Launch MVP for secure QR code management web app
- Enable internal teams to generate and manage QR codes securely
- Eliminate reliance on third-party QR code tools
- Ensure QR code reproducibility and user-specific management
- Provide easy PDF export for physical deployment

*Note: The app is responsible only for QR code generation and management. Generated QR codes will be used in day-to-day work with AR devices, but the app itself does not interact or integrate with AR devices.*

### Background Context
Teams require a reliable, in-house solution for generating and managing QR codes. Current third-party tools lack user-specific data management, reproducibility, and security. This project delivers a secure, user-authenticated web app built with ASP.NET Core MVC (.NET 10), allowing users to generate, store, and manage QR codes, with PDF export capabilities for physical deployment.

### Change Log
| Date       | Version | Description       | Author |
|------------|---------|-------------------|--------|
| 2026-02-11 | 1.0     | Initial PRD draft | John   |

## Requirements

### Functional Requirements
- User registration, login, and password reset
- Create QR code from a user-input string (up to 100 ASCII alphanumeric symbols) via a form
- User selects Error Correction Code (ECC) level (L, M, Q, H; default: M) and QR version (1–10; default: 5)
- Application validates input and provides feedback if the string cannot be encoded with the selected ECC/version
- Store QR code parameters in the database to allow recreation when needed, including an optional Notes field limited to 300 characters. Each QR code is associated with exactly one user (many-to-one relationship).
- Browse/filter user’s QR codes
- Generate and download PDF with SVG QR code

### Non-Functional Requirements
- Secure user authentication and data isolation
- Fast QR code generation and PDF export
- Support for modern browsers (Edge, Chrome, Firefox) on Windows OS
- On-premises hosting
- Minimal support and documentation (internal use)

## MVP Scope

### Core Features (Must Have)
- User registration, login, and password reset
- Create QR code from string input with version/encoding options
- Store QR code parameters in the database to allow recreation when needed. Each QR code is associated with exactly one user (many-to-one relationship).
- Browse/filter user’s QR codes
- Generate and download PDF with SVG QR code

### Out of Scope for MVP
- Multi-language support
- Advanced analytics/dashboard
- Bulk QR code generation
- API access

### MVP Success Criteria
- Users can register, create, store, and export QR codes without errors
- All core features work reliably for at least 90% of test cases

## Technical Considerations
- Platform: ASP.NET Core 10 MVC web application
- Browser/OS: Modern browsers (Edge, Chrome, Firefox) on Windows OS
- Performance: Fast QR code generation and PDF export for single codes
- Frontend: Razor views (MVC)
- Backend: ASP.NET Core 10
- Database: SQL Server
- Hosting: On-premises
- Use ASP.NET Core Identity for user management
- Use Entity Framework Core as ORM
- Use QuestPDF for PDF export
- Standard MVC project structure, monolithic web app
- No external integrations beyond core libraries (QR code, PDF)
- Security: User authentication/authorization, each QR code associated with one user

## Constraints & Assumptions
- Minimal budget, internal project (no external funding)
- Tight timeline: complete MVP as soon as possible
- Small team or single developer
- Only core features in scope, no post-MVP development
- Users have access to modern browsers and Windows OS
- No need for mobile or cross-platform support
- No integration with external systems required
- Security is limited to user authentication and data isolation

## Risks & Open Questions
- Security risks if authentication is not properly configured. The app is intended for local/internal use only and will not be public. Security best practices should be followed, but without overengineering.
- Dependency on third-party QR code and PDF libraries for long-term compatibility. The app is expected to have limited updates due to its narrow, internal use.
- Regulatory/compliance: No requirements beyond basic authentication.
- Library compatibility: .NET 10 is LTS; all required libraries are expected to be compatible.
- Backup/DR: No automated plan; a periodic database-level backup job will be set up. The information is not critical, and the app is not public.
