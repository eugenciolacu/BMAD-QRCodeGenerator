# Epic 4: Export & Download

## Epic ID
EPIC-004

## Description
Enable users to download their QR codes as PDF documents (with embedded SVG) and as standalone image files (SVG or PNG). Downloads are generated on demand (not stored), and ownership is validated before serving any file.

## Business Value
Enables physical deployment of QR codes — users can print PDFs for use in AR/VR workflows (e.g., HoloLens 2 spatial anchoring). Image download provides flexibility for digital use.

## Acceptance Criteria
1. User can download a QR code as a PDF from the details view
2. PDF contains the QR code rendered as SVG with relevant metadata (text, ECC, version, date)
3. PDF is generated on demand using QuestPDF (not stored in database)
4. User can download a QR code as an image (SVG or PNG) from the details view
5. Image is generated on demand (not stored)
6. Download endpoints validate ownership — only the owner can download
7. Downloads require authentication
8. PDF/image generation completes in under 1 second for typical input
9. Downloaded files have meaningful filenames

## Stories
| Story | Title | Priority |
|-------|-------|----------|
| [4.1](../stories/4.1.pdf-export.md) | Download QR Code as PDF | Must Have |
| [4.2](../stories/4.2.image-download.md) | Download QR Code as Image (SVG/PNG) | Should Have |

## Dependencies
- EPIC-001 (project setup)
- EPIC-002 (authentication)
- EPIC-003 (QR codes must exist to be exported)

## Risks
- QuestPDF license considerations (community edition is free for small revenue)
- SVG rendering fidelity — must verify QR codes are scannable from printed PDFs

## Source Requirements
- [PRD](../prd.md) — Functional Requirements: Generate and download PDF with SVG QR code
- [Architecture](../architecture.md) — §3.2 Backend, §3.6 Controller Routes (DownloadPdf, DownloadImage), §4.3 PDF Export
- [Front-End Spec](../front-end-spec.md) — Download buttons on Details page
