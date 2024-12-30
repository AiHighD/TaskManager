using Microsoft.EntityFrameworkCore;
using TasksManager.Data.Entities;

namespace TasksManager.Data;

public class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TasksConfiguration());
    }
    
    public DbSet<Tasks> Tasks { get; set; } = null!;

    public DbSet<Progress> Progress { get; set; } = null!;

    public DbSet<Documents> Documents { get; set; } = null!;

    public DbSet<Attachments> Attachments { get; set; } = null!;

}



