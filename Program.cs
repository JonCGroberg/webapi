using Hcso.Todo;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Database>(opt => opt.UseInMemoryDatabase("Todos"));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

WebApplication app = builder.Build();

// API Routes
app.MapGet("/api/todos", async (Database db) =>
    await db.Todos.ToListAsync());

app.MapGet("/api/todos/{id}", async (int id, Database db) =>
{
    var todo = await db.Todos.FindAsync(id);

    return todo is Todo ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/api/todos", async (Todo todo, Database db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created("/api/todos/", todo);
});

app.MapPut("/api/todos/{id}", async (int id, Todo newTodo, Database db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is Todo)
    {
        db.Todos.Update(newTodo);

        todo.Name = newTodo.Name;
        todo.IsComplete= newTodo.IsComplete;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    else return Results.NotFound();
});

app.MapDelete("/api/todos/{id}", async (int id, Database db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is Todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    else return Results.NotFound();
});

app.Run();