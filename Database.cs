using Microsoft.EntityFrameworkCore;

namespace Hcso.Todo;
class Database(DbContextOptions<Database> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
}