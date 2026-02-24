# Non-Functional Requirements (NFR) Assessment

## Document Info
| Field | Value |
|-------|-------|
| Project | QR Code Web App (ASP.NET Core 8 MVC) |
| Date | 2026-02-13 |
| Source | PRD §Non-Functional, Architecture §9, Brief §Goals |

---

## NFR-01: Performance

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | QR code generation and PDF export must complete quickly |
| **Target** | < 1 second per operation for typical input |
| **Scope** | Single-user operations (no concurrent load targets for MVP) |
| **Measurement** | Server-side timing from request to response |
| **Acceptance** | 95% of operations complete in under 1 second |
| **Test Approach** | Manual timing + optional stopwatch logging in services |
| **Stories** | 3.1 (QR generation), 4.1 (PDF export), 4.2 (Image download) |

## NFR-02: Security

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | Secure user authentication and strict data isolation |
| **Targets** | |
| - Authentication | ASP.NET Core Identity with strong password policy |
| - Data Isolation | Every DB query filtered by current UserId |
| - Session | HttpOnly, Secure, SameSite cookies; 30-min sliding timeout |
| - CSRF | Anti-forgery tokens on all POST actions |
| - XSS | Razor auto-encoding; Content-Security-Policy headers recommended |
| - Logging | Auth events logged; no sensitive data in logs |
| **Test Approach** | Security-focused test cases (T-020, T-035, T-040, T-042, T-043, T-052, T-055) |
| **Stories** | 2.1, 2.2, 2.3, 3.1–3.4, 4.1, 4.2 |

## NFR-03: Usability

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | Intuitive UI with minimal learning curve |
| **Targets** | |
| - Task Time | Generate and store a QR code in under 1 minute |
| - Success Rate | 95%+ successful PDF exports without errors |
| - UI Framework | Bootstrap 5, responsive, centered forms (max 600px) |
| - Feedback | Inline validation, success/error alerts with clear messages |
| - Navigation | Primary nav: Dashboard, Create, My QR Codes, Settings, Logout |
| **Test Approach** | Manual walkthrough of all user flows |
| **Stories** | All UI stories |

## NFR-04: Browser Compatibility

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | Support modern browsers on Windows OS |
| **Supported Browsers** | Microsoft Edge, Google Chrome, Mozilla Firefox (latest 2 versions) |
| **Not Supported** | Internet Explorer, Safari, mobile browsers |
| **Test Approach** | Manual cross-browser smoke test on each supported browser |
| **Stories** | All UI stories |

## NFR-05: Availability

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | Available during business hours (not 24/7) |
| **Target** | Best-effort during work hours; downtime for maintenance acceptable |
| **Hosting** | On-premises Windows server, IIS or Kestrel |
| **Monitoring** | No external monitoring for MVP; structured logging only |
| **Backup** | Periodic SQL Server backup job (manual, not automated) |
| **Stories** | 1.1 (project setup, hosting config) |

## NFR-06: Maintainability

| Attribute | Specification |
|-----------|--------------|
| **Requirement** | Codebase is clean, standard, and easy to maintain |
| **Targets** | |
| - Architecture | Standard ASP.NET Core MVC patterns |
| - Separation | Controllers, Services, Data, Models, Views in separate folders |
| - Configuration | Centralized in appsettings.json |
| - Documentation | Architecture doc, this QA doc set, inline code comments |
| - Testing | Unit and integration tests recommended for critical paths |
| **No Post-MVP** | No further development planned; maintainability supports bug fixes only |
| **Stories** | 1.1 (project structure) |

---

## Summary

| NFR | Status | Risk Level |
|-----|--------|------------|
| Performance | Defined & measurable | Low |
| Security | Comprehensive controls defined | Medium (config errors) |
| Usability | UX spec and wireframes complete | Low |
| Browser Compatibility | Scope limited to 3 modern browsers | Low |
| Availability | Business hours, best-effort | Low |
| Maintainability | Standard patterns, no post-MVP | Low |
