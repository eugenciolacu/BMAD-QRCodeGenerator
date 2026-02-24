# Risk Assessment: QR Code Management Web App

## Document Info
| Field | Value |
|-------|-------|
| Project | QR Code Web App (ASP.NET Core 8 MVC) |
| Date | 2026-02-13 |
| Source | PRD §Risks, Architecture §11, Brief §Risks |

## Risk Register

### R-001: Authentication Misconfiguration
- **Severity:** High
- **Likelihood:** Medium
- **Impact:** Unauthorized access to QR code data; data leakage between users
- **Mitigation:**
  - Use ASP.NET Core Identity defaults (salted hashing, lockout, token management)
  - Enforce data isolation via UserId filtering on every query
  - Never return 403 — use 404 to avoid revealing existence of other users' data
  - Code review all authorization checks before release
- **Owner:** Developer
- **Status:** Open

### R-002: Weak Password Policy
- **Severity:** Medium
- **Likelihood:** Low
- **Impact:** Brute-force or dictionary attacks on user accounts
- **Mitigation:**
  - Configure Identity password options: min 8 chars, require uppercase, lowercase, digit, special char
  - Account lockout after 5 failed attempts
- **Owner:** Developer
- **Status:** Open

### R-003: Data Loss (No Automated Backup)
- **Severity:** High
- **Likelihood:** Medium
- **Impact:** Loss of all QR code data and user accounts
- **Mitigation:**
  - Configure scheduled SQL Server backup job
  - Document manual restore procedure
  - Data is not critical and can be regenerated (lower actual impact)
- **Owner:** DevOps / Admin
- **Status:** Open

### R-004: Third-Party Library Compatibility
- **Severity:** Medium
- **Likelihood:** Low
- **Impact:** QR code generation or PDF export breaks on library update
- **Mitigation:**
  - Use .NET 8 LTS (supported until Nov 2026)
  - Pin library versions in project file
  - No post-MVP development planned — limited exposure to breaking changes
- **Owner:** Developer
- **Status:** Open

### R-005: QR Code Encoding Failures
- **Severity:** Low
- **Likelihood:** Medium
- **Impact:** User cannot create QR code with certain input/ECC/version combinations
- **Mitigation:**
  - Validate input against library capabilities before saving
  - Show clear, actionable error message to user
  - Document supported input ranges
- **Owner:** Developer
- **Status:** Open

### R-006: Limited Test Coverage
- **Severity:** Medium
- **Likelihood:** High
- **Impact:** Bugs ship to production undetected
- **Mitigation:**
  - Prioritize testing of security-critical paths (auth, data isolation)
  - Manual test pass for all key scenarios before release
  - App is internal/low-stakes — risk tolerance is higher
- **Owner:** Developer / QA
- **Status:** Open

### R-007: No Email Service for Password Reset
- **Severity:** Low
- **Likelihood:** Certain (by design)
- **Impact:** Password reset token must be delivered via alternative means (on-screen, log)
- **Mitigation:**
  - For MVP: display reset token/link on confirmation page or log it
  - Document the limitation for admin/support staff
  - Can add SMTP later if needed
- **Owner:** Developer
- **Status:** Accepted

## Risk Matrix

| | Low Impact | Medium Impact | High Impact |
|---|-----------|--------------|-------------|
| **High Likelihood** | | R-006 | |
| **Medium Likelihood** | R-005 | | R-001, R-003 |
| **Low Likelihood** | | R-002, R-004 | |
| **Certain** | R-007 | | |
