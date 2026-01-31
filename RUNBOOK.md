# Runbook — SupportEngineerChallenge

> Update this file as part of the exercise.

## Service overview
- **Service:** SupportEngineerChallenge.Api
- **Purpose:** Minimal task tracker (create + list tasks)
- **Data store:** SQLite (`app.db` in the API working directory)

## Common commands

**Run locally**
```bashtestt
cd src/SupportEngineerChallenge.Api
dotnet run
```

**Run tests**
```bash
dotnet test
```
**Comments**

Initially ran into an error when attempting to run dotnet test:

"..error CS0246: the type of namespace name 'IClassFixture<>' could not be found (are you missing a using directive or an assemably reference?).."

Issue resolved via editing "TaskApiTests.cs" file in adding "using Xunit;" to the top of the page.


## Key endpoints
- `GET /api/tasks?userId={id}&limit={n}`
- `POST /api/tasks`

## Troubleshooting checklist (starter)

### “Create task fails with 500”
- Check API logs in console.
```initating Swagger POST /api/tasks endpoint produces server 500 error via request URL "http://localhost:5000/api/tasks". On webapp it throws server 500
error every 2-4 task additions, inconsistent pattern but occurs in this general sequence.```
- Verify request payload and headers.
- Look for unhandled exceptions in `POST /api/tasks`.
```Introduction of perhaps a try-catch block in the .cs page code for when users attempt to create a task without a description could help produce a more
helpful error test to user without also introducing compliance/code leak on frontend.```

### “Tasks list is slow”
- Confirm dataset size (seed can be large).
```query API get endpoint is set for 50 limit display on UI static page, but more than 50 populate, including duplicates(need duplicate check in code,
and redundancy check for results which populate same instances. With each page refresh, the same instance row value replaces each results below it
interactively upon each manual page refresh.```
- Inspect how the list endpoint fetches and filters data.
- Review query patterns and database usage.

### “Duplicates / wrong order after refresh”
- Compare API response vs UI rendering.
- Check the UI state update logic during refresh.
- Verify how the list is merged and ordered.

## Verification steps (starter)
- Create tasks from UI and via Swagger.
- Refresh tasks repeatedly; confirm no duplicates and ordering is correct.
- Validate list endpoint returns only requested user's tasks.

## Rollback / mitigation ideas (starter)
- Roll back to last known good version.
- Temporarily disable problematic client behavior (feature flag / UI change).
- Add guardrails (e.g. input validation, error handling) to prevent unhandled exceptions.
