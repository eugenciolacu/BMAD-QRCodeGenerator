# Requirements Traceability Matrix

## Document Info
| Field | Value |
|-------|-------|
| Project | QR Code Web App (ASP.NET Core 8 MVC) |
| Date | 2026-02-13 |
| Source | PRD, Architecture, Brief, Front-End Spec |

## Traceability: PRD → Epics → Stories → Tests

### Functional Requirements

| Req ID | PRD Requirement | Epic | Stories | Test Cases |
|--------|----------------|------|---------|------------|
| FR-01 | User registration | EPIC-002 | 2.1 | T-010, T-011, T-012, T-013 |
| FR-02 | User login | EPIC-002 | 2.2 | T-014, T-015, T-016, T-020 |
| FR-03 | Password reset | EPIC-002 | 2.3 | T-017, T-018, T-019 |
| FR-04 | Create QR code from string input | EPIC-003 | 3.1 | T-030, T-031, T-032, T-033 |
| FR-05 | Select ECC level and QR version | EPIC-003 | 3.1 | T-030, T-034 |
| FR-06 | Validate input, provide feedback | EPIC-003 | 3.1 | T-031, T-032, T-033, T-034 |
| FR-07 | Store QR code parameters in DB | EPIC-003 | 3.1 | T-030 |
| FR-08 | Browse/filter user's QR codes | EPIC-003 | 3.2 | T-035, T-036, T-037 |
| FR-09 | Generate and download PDF with SVG QR | EPIC-004 | 4.1 | T-050, T-051, T-056 |

### Non-Functional Requirements

| Req ID | PRD/Arch Requirement | Epic | Stories | Test Cases |
|--------|---------------------|------|---------|------------|
| NFR-01 | Secure authentication & data isolation | EPIC-002, EPIC-003 | 2.1–2.3, 3.1–3.4 | T-020, T-035, T-040, T-042, T-052, T-055 |
| NFR-02 | Fast QR generation & PDF export (<1s) | EPIC-003, EPIC-004 | 3.1, 4.1 | T-056 |
| NFR-03 | Modern browser support (Edge, Chrome, Firefox) | All | All | Manual cross-browser |
| NFR-04 | On-premises hosting | EPIC-001 | 1.1 | Deployment verification |

### Architecture Requirements

| Req ID | Architecture Requirement | Epic | Stories |
|--------|------------------------|------|---------|
| AR-01 | ASP.NET Core 8 MVC | EPIC-001 | 1.1 |
| AR-02 | SQL Server + EF Core | EPIC-001 | 1.2 |
| AR-03 | ASP.NET Core Identity | EPIC-002 | 2.1, 2.2, 2.3 |
| AR-04 | Net.Codecrete.QrCodeGenerator | EPIC-003 | 3.1, 3.3 |
| AR-05 | QuestPDF | EPIC-004 | 4.1 |
| AR-06 | Anti-forgery tokens on POST | EPIC-002, EPIC-003 | All POST stories |
| AR-07 | Structured logging | EPIC-001 | 1.1 |
| AR-08 | Global error handling | EPIC-001 | 1.1 |
| AR-09 | Bootstrap 5 UI | All | All UI stories |

## Coverage Summary
- **Functional Requirements:** 9/9 traced (100%)
- **Non-Functional Requirements:** 4/4 traced (100%)
- **Architecture Requirements:** 9/9 traced (100%)
- **Total Test Cases:** 28 defined across all epics
