using APIproject_1934163.Models;
using Microsoft.EntityFrameworkCore;

namespace APIproject_1934163.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DoingTask> DoingTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
