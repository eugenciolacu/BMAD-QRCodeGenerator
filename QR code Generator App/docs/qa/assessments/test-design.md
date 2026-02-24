# Test Design: QR Code Management Web App

## Document Info
| Field | Value |
|-------|-------|
| Project | QR Code Web App (ASP.NET Core 8 MVC) |
| Date | 2026-02-13 |
| Source | PRD, Architecture §7, Front-End Spec |

## Test Strategy

### Test Levels
1. **Unit Tests** — Controllers, services (QRCodeService, PdfExportService), view models
2. **Integration Tests** — Database operations, Identity flows, EF Core queries
3. **Functional/UI Tests** — Manual end-to-end tests of all user flows
4. **Security Tests** — Authentication, authorization, data isolation

### Test Priority
Given tight timeline and single developer, testing effort is prioritized:
1. **P1 (Critical):** Authentication, data isolation, QR code CRUD
2. **P2 (High):** PDF export, image download, input validation
3. **P3 (Medium):** UI/UX consistency, edge cases, performance

---

## Test Scenarios by Epic

### EPIC-001: Project Setup & Foundation
| ID | Scenario | Type | Priority |
|----|----------|------|----------|
| T-001 | Application builds without errors | Build | P1 |
| T-002 | Application starts and serves landing page | Smoke | P1 |
| T-003 | Database migrations apply successfully | Integration | P1 |
| T-004 | QRCodes table schema matches spec | Integration | P1 |

### EPIC-002: User Authentication
| ID | Scenario | Type | Priority |
|----|----------|------|----------|
| T-010 | Register with valid credentials → success | Functional | P1 |
| T-011 | Register with weak password → error | Functional | P1 |
| T-012 | Register with duplicate email → error | Functional | P1 |
| T-013 | Register with mismatched passwords → error | Functional | P2 |
| T-014 | Login with valid credentials → Dashboard | Functional | P1 |
| T-015 | Login with invalid credentials → generic error | Functional | P1 |
| T-016 | Logout → session invalidated | Functional | P1 |
| T-017 | Forgot password flow → token generated | Functional | P2 |
| T-018 | Password reset with valid token → password changed | Functional | P2 |
| T-019 | Password reset with expired token → error | Functional | P2 |
| T-020 | Unauthenticated access to /QRCodes → redirect to Login | Security | P1 |

### EPIC-003: QR Code CRUD
| ID | Scenario | Type | Priority |
|----|----------|------|----------|
| T-030 | Create QR code with valid input → saved, redirect | Functional | P1 |
| T-031 | Create QR code with empty text → validation error | Functional | P1 |
| T-032 | Create QR code with >100 chars → validation error | Functional | P1 |
| T-033 | Create QR code with non-ASCII chars → validation error | Functional | P2 |
| T-034 | Create QR code with incompatible ECC/version → error msg | Functional | P1 |
| T-035 | Browse QR codes — only user's own codes visible | Security | P1 |
| T-036 | Browse QR codes — search filter works | Functional | P2 |
| T-037 | Browse QR codes — pagination works | Functional | P2 |
| T-038 | View QR code details — all fields displayed | Functional | P1 |
| T-039 | View QR code details — preview rendered | Functional | P2 |
| T-040 | Access another user's QR code → 404 | Security | P1 |
| T-041 | Delete QR code with confirmation → removed | Functional | P1 |
| T-042 | Delete another user's QR code → 404 | Security | P1 |
| T-043 | Delete without anti-forgery token → 400 | Security | P2 |

### EPIC-004: Export & Download
| ID | Scenario | Type | Priority |
|----|----------|------|----------|
| T-050 | Download PDF for owned QR code → valid PDF | Functional | P1 |
| T-051 | PDF contains correct QR code and metadata | Functional | P1 |
| T-052 | Download PDF for another user's QR code → 404 | Security | P1 |
| T-053 | Download image (SVG) → valid SVG file | Functional | P2 |
| T-054 | Download image (PNG) → valid PNG file | Functional | P2 |
| T-055 | Download image for another user's QR code → 404 | Security | P1 |
| T-056 | PDF generation < 1 second | Performance | P3 |

---

## Test Environment
- OS: Windows (modern browsers: Edge, Chrome, Firefox)
- Database: SQL Server (local instance)
- Hosting: Kestrel (dev) / IIS (staging)

## Test Data
- Minimum 2 user accounts for data isolation testing
- QR codes with varying text lengths, ECC levels, and versions
- Edge case inputs: empty string, 100-char string, special characters
