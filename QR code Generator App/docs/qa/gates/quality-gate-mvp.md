# Quality Gate: MVP Release

## Gate ID
QG-MVP

## Purpose
Final quality gate before deploying the MVP to production (on-premises). Ensures all core features work, security is properly configured, and the application is ready for internal use.

## Entry Criteria
- [ ] All 4 epics completed (EPIC-001 through EPIC-004)
- [ ] All 10 stories marked as "Done"
- [ ] Per-story quality gate (QG-DEV) passed for every story

---

## Functional Verification
- [ ] **User Registration:** Users can register with email/password; validation works
- [ ] **User Login:** Users can log in; invalid credentials show generic error
- [ ] **User Logout:** Session invalidated; redirect to login
- [ ] **Password Reset:** Token generated; password can be reset
- [ ] **Create QR Code:** Valid input creates and stores QR code; invalid input shows error
- [ ] **Browse QR Codes:** User sees only their own QR codes; search and pagination work
- [ ] **QR Code Details:** All metadata and preview displayed correctly
- [ ] **Delete QR Code:** Confirmation required; QR code removed from database
- [ ] **PDF Download:** Valid PDF with embedded QR code and metadata
- [ ] **Image Download:** Valid SVG/PNG image of QR code

## Security Verification
- [ ] Authentication required for all /QRCodes/* routes
- [ ] Data isolation: User A cannot access User B's QR codes (returns 404)
- [ ] Strong password policy enforced
- [ ] Anti-forgery tokens validated on all POST actions
- [ ] Secure cookies configured (HttpOnly, Secure, SameSite)
- [ ] HTTPS enforced
- [ ] No sensitive data in logs or error pages
- [ ] Generic error messages on login failure (no information leakage)

## Non-Functional Verification
- [ ] QR code generation completes in < 1 second
- [ ] PDF export completes in < 1 second
- [ ] Application works on Edge, Chrome, Firefox (Windows)
- [ ] Structured logging produces meaningful log entries
- [ ] Global error handler returns friendly error page (not stack trace)

## Infrastructure & Deployment
- [ ] appsettings.json configured for production (connection string, logging level)
- [ ] Database migrations applied to production database
- [ ] SQL Server backup job scheduled
- [ ] Application deployed to IIS or Kestrel on target server
- [ ] HTTPS certificate configured
- [ ] Application accessible from internal network

## Documentation
- [ ] Architecture document up-to-date
- [ ] This QA documentation complete
- [ ] Basic user instructions available (how to register, create QR code, export PDF)
- [ ] Admin/deployment notes documented (how to deploy, configure, backup)

---

## Exit Criteria
- [ ] All sections above pass
- [ ] No open critical or blocker issues
- [ ] Stakeholder sign-off obtained
- [ ] Application is live and accessible to internal users

## Sign-Off

| Role | Name | Date | Status |
|------|------|------|--------|
| Product Owner | | | |
| Developer | | | |
| Stakeholder | | | |
