# Quality Gate: Development Complete (Per Story)

## Gate ID
QG-DEV

## Purpose
Verify each story meets its acceptance criteria and coding standards before marking as Done.

## Entry Criteria
- [ ] Story status is "InProgress" or "Review"
- [ ] All tasks and subtasks in the story are checked off
- [ ] Code compiles without errors or warnings

## Quality Checks
- [ ] All acceptance criteria verified (manually or with tests)
- [ ] Input validation works client-side and server-side
- [ ] Data isolation enforced (user can only access own data)
- [ ] Anti-forgery tokens present on all POST forms
- [ ] Error handling: no unhandled exceptions; user-friendly error messages
- [ ] Logging: critical actions logged (auth events, CRUD operations)
- [ ] UI matches front-end spec wireframes and Bootstrap 5 styling
- [ ] Code follows standard ASP.NET Core MVC patterns (Controllers, Services, Views, Models)
- [ ] No hardcoded credentials, connection strings, or secrets in code

## Exit Criteria
- [ ] All quality checks pass
- [ ] Story can be marked as "Done"

## Applicable To
All stories across all epics
