# Epic 3: QR Code CRUD Operations

## Epic ID
EPIC-003

## Description
Implement full QR code lifecycle: create QR codes from user input with configurable ECC and version, validate input constraints, store parameters in the database (including an optional Notes field limited to 300 characters), browse/filter the user's codes, view details, and delete codes. All operations enforce data isolation (users can only access their own QR codes).

## Business Value
Core application functionality — enables users to generate, store, manage, and delete QR codes for their AR/VR workflows. This is the primary value driver of the MVP.

## Acceptance Criteria
1. Create QR Code form accepts up to 100 ASCII alphanumeric characters
2. User can select ECC level (L, M, Q, H; default: M)
3. User can select QR version (1–10; default: 5)
4. Form validates input and displays clear feedback if encoding is not possible with selected ECC/version
5. QR code parameters are saved to the database (not the image itself), including an optional Notes field limited to 300 characters
6. QR code is generated on-the-fly using Net.Codecrete.QrCodeGenerator for preview
7. User can browse a paginated, filterable list of their own QR codes
8. User can view details of a specific QR code (preview + metadata, including Notes up to 300 characters)
9. User can delete a QR code with confirmation
10. All operations require authentication; users cannot access/modify other users' QR codes
11. Anti-forgery tokens on all POST actions
12. Critical actions (create, delete) are logged

## Stories
| Story | Title | Priority |
|-------|-------|----------|
| [3.1](../stories/3.1.create-qr-code.md) | Create QR Code | Must Have |
| [3.2](../stories/3.2.browse-filter-qr-codes.md) | Browse & Filter QR Codes | Must Have |
| [3.3](../stories/3.3.qr-code-details.md) | View QR Code Details | Must Have |
| [3.4](../stories/3.4.delete-qr-code.md) | Delete QR Code | Must Have |

## Dependencies
- EPIC-001 (database schema)
- EPIC-002 (authentication — user must be logged in)

## Risks
- Input validation edge cases (special characters, encoding limits)
- QR library may silently fail for certain ECC/version/input combinations — must handle gracefully

## Source Requirements
- [PRD](../prd.md) — Functional Requirements: QR code creation, storage, browse/filter
- [Architecture](../architecture.md) — §3.2 Backend, §3.5 Database Schema, §3.6 Controller Routes, §7 Validation
- [Front-End Spec](../front-end-spec.md) — Create QR Code Page, My QR Codes Page, Details Page wireframes
