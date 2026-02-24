# Project Brief: QR Code Web App (ASP.NET Core 10 MVC)

## Executive Summary
A secure web application built with ASP.NET Core MVC (.NET 10) that allows users to generate, store, and manage QR codes, with user authentication and PDF export features.

- **Primary Problem:** Teams working with HoloLens 2 and similar devices need a reliable, in-house solution for generating and managing QR codes, avoiding reliance on third-party services.
- **Target Market:** Organizations and developers using HoloLens 2 or similar AR devices for spatial computing, prototyping, or testing.
- **Key Value Proposition:** Enables secure, user-specific QR code management, reproducibility, and easy PDF export, tailored for AR/VR workflows.

## Proposed Solution
A secure, user-authenticated web app built with ASP.NET Core MVC (.NET 10) for generating, storing, and managing QR codes. Each user can create QR codes with custom parameters, store them in a personal database, and export them as SVG or PDF. Unlike generic online QR generators, this solution offers user-specific data management, reproducibility, and integration with AR workflows (e.g., HoloLens 2). The focus on security, user ownership, and PDF export for physical deployment addresses gaps in current third-party tools.

## Target Users
- **Profile:** Developers, engineers, and teams working with HoloLens 2 or similar AR devices in organizations focused on spatial computing, prototyping, or research.
- **Behaviors:** Frequently generate and use QR codes for spatial anchoring, object spawning, or workflow automation; currently rely on third-party QR code generators.
- **Needs/Pain Points:** Need secure, reproducible, and user-specific QR code management; want to avoid third-party dependencies; require easy PDF export for physical deployment.
- **Goals:** Streamline AR/VR testing and deployment, ensure QR code reproducibility, and maintain control over QR code data.

## Goals & Success Metrics
- **Business Objectives:**
  - Launch MVP as soon as possible (target: one sprint)
  - Enable internal teams to generate and manage QR codes securely
  - Eliminate reliance on third-party QR code tools for internal workflows
- **User Success Metrics:**
  - Users can generate and store a QR code in under 1 minute
  - 95%+ successful PDF exports without errors
  - Positive user feedback from internal teams on ease of use
- **KPIs:**
  - Number of QR codes generated per user (target: 10+ per team member)
  - Percentage of QR code operations completed without errors (target: 95%+)
  - Support requests related to QR code errors (target: <5% of total sessions)
  - Internal adoption rate across relevant departments

## MVP Scope
  - User registration, login, and password reset (ensures secure access)
  - Create QR code from a user-input string (up to 100 ASCII alphanumeric symbols) via a form
  - User selects Error Correction Code (ECC) level (L, M, Q, H; default: M) and QR version (1–10; default: 5)
  - Application validates input and provides feedback if the string cannot be encoded with the selected ECC/version
  - Store QR code parameters per user in SQL Server (enables reproducibility), including an optional Notes field limited to 300 characters
  - Browse/filter user’s QR codes (usability)
  - Generate and download PDF with SVG QR code (deployment/printing)
- **Out of Scope for MVP:**
  - Multi-language support
  - Advanced analytics/dashboard
  - Bulk QR code generation
  - API access
- **MVP Success Criteria:**
  - Users can register, create, store, and export QR codes without errors
  - All core features work reliably for at least 90% of test cases

## Post-MVP Vision
No further development is planned beyond the MVP. The project will be considered complete once the core functionality is delivered.

## Technical Considerations
- **Platform Requirements:**
  - Target Platforms: Web (ASP.NET Core 10 MVC)
  - Browser/OS Support: Modern browsers (Edge, Chrome, Firefox), Windows OS preferred
  - Performance Requirements: Fast QR code generation and PDF export for single codes
- **Technology Preferences:**
  - Frontend: Razor views (MVC)
  - Backend: ASP.NET Core 10
  - Database: SQL Server
  - PDF Export: QuestPDF
  - Hosting/Infrastructure: On-premises
- **Architecture Considerations:**
  - Repository Structure: Standard MVC project structure
  - Service Architecture: Monolithic web app
  - Integration Requirements: None beyond core libraries (QR code, PDF)
  - Security/Compliance: User authentication/authorization, data isolation per user

## Constraints & Assumptions
- **Constraints:**
  - Budget: Minimal, internal project (no external funding)
  - Timeline: Complete MVP as soon as possible
  - Resources: Single developer or small team
  - Technical: Only core features, no post-MVP development
- **Key Assumptions:**
  - Users will have access to modern browsers and Windows OS
  - No need for mobile or cross-platform support
  - No integration with external systems required
  - Security is limited to user authentication and data isolation

## Risks & Open Questions
- **Key Risks:**
  - Dependency on Net.Codecrete.QrCodeGenerator and PDF libraries for long-term compatibility
  - Security risks if authentication is not properly configured
  - Minimal user support and documentation due to limited scope
- **Open Questions (clarified):**
  - Regulatory/compliance: No special requirements; only email and password stored
  - Deployment: On-premises only
  - Backup/DR: No automated plan; manual database management (e.g., SQL jobs)
- **Areas Needing Further Research:**
  - Confirm continued compatibility and support for Net.Codecrete.QrCodeGenerator and chosen PDF library with .NET 10

