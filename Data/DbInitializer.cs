using Microsoft.EntityFrameworkCore;
using TasksManager.Data.Entities;

namespace TasksManager.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new TasksDbContext(serviceProvider.GetRequiredService<DbContextOptions<TasksDbContext>>()))
            {
                // Seed Tasks
                if (!context.Tasks.Any())
                {
                    context.Tasks.AddRange(
                        new Tasks
                        {
                            Task_Name = "Learn ASP.NET Core",
                            Topic = ".NET Programming",
                            Description = "Learn ASP.NET Core from scratch",
                            StartTime = DateTime.Today,
                            EndTime = DateTime.Today.AddDays(30),
                            Status = "In Progress"
                        },
                        new Tasks
                        {
                            Task_Name = "Learn Java Spring Boot",
                            Topic = "Java Programming",
                            Description = "Learn Java Spring Boot from scratch",
                            StartTime = DateTime.Today,
                            EndTime = DateTime.Today.AddDays(30),
                            Status = "In Progress"
                        },
                        new Tasks
                        {
                            Task_Name = "Learn Laravel",
                            Topic = "PHP Programming",
                            Description = "Learn Laravel from scratch",
                            StartTime = DateTime.Today,
                            EndTime = DateTime.Today.AddDays(30),
                            Status = "In Progress"
                        }
                    );
                    context.SaveChanges();
                }

            }
        }
    }
}
