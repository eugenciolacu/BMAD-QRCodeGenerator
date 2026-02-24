# Epic 2: User Authentication & Account Management

## Epic ID
EPIC-002

## Description
Implement secure user registration, login, logout, and password reset using ASP.NET Core Identity. Enforce strong password policies, session management, and HTTPS. Provide clear, Bootstrap 5–styled authentication pages per the front-end spec.

## Business Value
Enables secure, per-user access to the application, ensuring data isolation and protecting QR code data. This is a prerequisite for all QR code management features.

## Acceptance Criteria
1. Users can register with email and strong password; validation errors shown inline
2. Users can log in with valid credentials and are redirected to Dashboard
3. Invalid login attempts show clear error messages without exposing sensitive info
4. Users can log out; session is invalidated
5. Users can request a password reset via "Forgot Password" flow
6. Passwords are stored as salted hashes (ASP.NET Core Identity default)
7. Strong password policy enforced (min length, complexity)
8. Anti-forgery tokens used on all POST forms
9. Secure cookies (HttpOnly, Secure, SameSite) are configured
10. Authentication events are logged (login, logout, failed attempts)
11. All authentication pages match the front-end spec wireframes and Bootstrap 5 styling

## Stories
| Story | Title | Priority |
|-------|-------|----------|
| [2.1](../stories/2.1.user-registration.md) | User Registration | Must Have |
| [2.2](../stories/2.2.user-login-logout.md) | User Login & Logout | Must Have |
| [2.3](../stories/2.3.password-reset.md) | Forgot Password / Password Reset | Must Have |
| [2.4](../stories/2.4.email-sender-service.md) | Email Sender Service for Registration & Password Reset | Should Have |

> Note: Story 2.4 was added after initial planning to enable registration confirmation and is required for story 2.3 (Forgot Password / Password Reset) to function correctly. This addresses a gap in the original Register implementation.

## Dependencies
- EPIC-001 (Project Setup & Foundation must be complete)

## Risks
- Security misconfiguration if Identity defaults are overridden incorrectly
- Password reset requires email sending capability (may use a simple local SMTP or token-based approach for internal use)

## Source Requirements
- [PRD](../prd.md) — Functional Requirements: User registration, login, password reset
- [Architecture](../architecture.md) — §4.1 User Authentication, §6 Security Model
- [Front-End Spec](../front-end-spec.md) — Authentication Pages, Wireframes
