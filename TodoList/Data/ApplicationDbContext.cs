using Microsoft.EntityFrameworkCore;
using TodoList.Shared.Models.Entities;

namespace TodoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks { get; set; }
    }
}
