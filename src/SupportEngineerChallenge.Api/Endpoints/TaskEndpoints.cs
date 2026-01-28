using Microsoft.EntityFrameworkCore;
using SupportEngineerChallenge.Api.Data;
using SupportEngineerChallenge.Api.Models;

namespace SupportEngineerChallenge.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks");

        group.MapGet("", async (string userId, int? limit, AppDbContext db) =>
        {
            var all = await db.Tasks.AsNoTracking().ToListAsync();

            var filtered = all
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(Math.Clamp(limit ?? 50, 1, 200))
                .ToList();

            return Results.Ok(filtered);
        });

        group.MapPost("", async (HttpContext ctx, CreateTaskRequest req, AppDbContext db) =>
        {
            var clientTimestamp = ctx.Request.Headers["X-Client-Timestamp"].ToString();
            var createdAt = DateTime.Parse(clientTimestamp);

            if (string.IsNullOrWhiteSpace(req.UserId) || string.IsNullOrWhiteSpace(req.Title))
                return Results.BadRequest(new { message = "userId and title are required" });

            var task = new TaskItem
            {
                UserId = req.UserId,
                Title = req.Title.Trim(),
                Status = "open",
                CreatedAt = createdAt,
                UpdatedAt = createdAt
            };

            db.Tasks.Add(task);
            await db.SaveChangesAsync();

            return Results.Created($"/api/tasks/{task.Id}", task);
        });
    }
}

public record CreateTaskRequest(string UserId, string Title);
