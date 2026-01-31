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

```Initially ran into an error when attempting to run dotnet test:

"..error CS0246: the type of namespace name 'IClassFixture<>' could not be found (are you missing a using directive or an assemably reference?).."

Issue resolved via editing "TaskApiTests.cs" file in adding "using Xunit;" to the top of the page. Running dotnet test produces no failed issues, 2 correct
in total, but seems to miss some tasks.```


## Key endpoints
- `GET /api/tasks?userId={id}&limit={n}`
- `POST /api/tasks`

## Troubleshooting checklist (starter)

### “Create task fails with 500”
- Check API logs in console.
```initating Swagger POST /api/tasks endpoint produces server 500 error via request URL "http://localhost:5000/api/tasks". On webapp it throws server 500
error every 2-4 task additions, inconsistent pattern but occurs in this general sequence.```
- Verify request payload and headers.
``` content-length: 0 
 date: Sat,31 Jan 2026 20:36:41 GMT 
 server: Kestrel   ```
- Look for unhandled exceptions in `POST /api/tasks`.
```Introduction of perhaps a try-catch block in the .cs page code for when users attempt to create a task without a description could help produce a more
helpful error test to user without also introducing compliance/code leak on frontend.```

### “Tasks list is slow”
- Confirm dataset size (seed can be large).
```query API get endpoint is set for 50 limit display on UI static page, but more than 50 populate, including duplicates(need duplicate check in code,
and redundancy check for results which populate same instances. With each page refresh, the same instance row value replaces each results below it
interactively upon each manual page refresh.```
- Inspect how the list endpoint fetches and filters data.
```Static UI page lists dulpicates and overrides results with first search result at top of page. Comparably with Swagger's GET endpoint, it returns correctly
50 results as coded in the task .cs and each value is unique. Seems to be a frontend issue with the display, compared to an API/backend problem it seems. ```
- Review query patterns and database usage.

### “Duplicates / wrong order after refresh”
- Compare API response vs UI rendering.
```UI displays duplicate and malformed order of tasks. Swagger GET endpoint shows proper order from ascending order and unique values. ```
- Check the UI state update logic during refresh.
```When refreshing and checking browser console, both on Google Chrome and Mozilla Firefox, the endpoint retrieves a limit query back of 50 from the GET
endpoint "http://localhost:5000/api/tasks?userId=user-001&limit=50" however the order is still misaligned, citing an issue with the ascending order aspect. This
is compared to the UI webpage displaying > 50 results.  ```
- Verify how the list is merged and ordered.
```Random order, with duplicates between 2 to 15 results for particular results, based on task ID row.   ```

## Verification steps (starter)
- Create tasks from UI and via Swagger.
```Static UI webapp does ingest new task entries with filled out text field for task title, as well as in Swagger but with constant error 500 codes.   ```
- Refresh tasks repeatedly; confirm no duplicates and ordering is correct.
- Validate list endpoint returns only requested user's tasks.

## Rollback / mitigation ideas (starter)
- Roll back to last known good version.
- Temporarily disable problematic client behavior (feature flag / UI change).
- Add guardrails (e.g. input validation, error handling) to prevent unhandled exceptions.
