# Epic 1: Project Setup & Foundation

## Epic ID
EPIC-001

## Description
Establish the ASP.NET Core 10 MVC project structure, configure Entity Framework Core with SQL Server, set up the database schema (including ASP.NET Core Identity tables and the QR Codes table), and ensure the foundational infrastructure is ready for feature development.

## Business Value
Provides the essential technical foundation upon which all application features depend. Without this infrastructure, no user-facing functionality can be delivered.

## Acceptance Criteria
1. ASP.NET Core 10 MVC project is created with standard folder structure (Controllers, Views, Models, Services, Data)
2. SQL Server database is configured and accessible via Entity Framework Core
3. ASP.NET Core Identity is integrated for user management
4. QR Codes table schema is created per architecture spec (Id, UserId FK, DecodedText, ErrorCorrectionLevel, QRVersion, CreatedAt, Notes)
5. EF Core migrations are generated and can be applied successfully
# 2026-02-16: Auto-migration on application startup implemented in Story 1.2
6. Application builds, runs, and serves a basic landing page
7. Required NuGet packages are installed (EF Core, Identity, QuestPDF, Net.Codecrete.QrCodeGenerator)
8. Configuration files (appsettings.json) contain connection string and app settings
9. Global error handling middleware is configured
10. Structured logging is configured using Serilog (COMPLETED)
# 2026-02-16: Structured logging with Serilog implemented and verified for Story 1.1 (by user)

## Stories
| Story | Title | Priority |
|-------|-------|----------|
| [1.1](../stories/1.1.project-init.md) | Initialize ASP.NET Core 10 MVC Project | Must Have |
| [1.2](../stories/1.2.database-setup.md) | Configure Database & EF Core Schema | Must Have |

## Dependencies
- None (this is the foundational epic)

## Risks
- Library compatibility issues with .NET 10 (mitigated: .NET 10 is LTS, all libraries confirmed compatible)

## Source Requirements
- [PRD](../prd.md) — Technical Considerations
- [Architecture](../architecture.md) — §2 Technology Stack, §3.3 Database, §3.5 Database Schema
