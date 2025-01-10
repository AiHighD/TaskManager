using Microsoft.AspNetCore.Identity;
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
        base.OnModelCreating(modelBuilder);

        // Áp dụng cấu hình tùy chỉnh cho các thực thể của bạn
        modelBuilder.ApplyConfiguration(new TasksConfiguration());

        // Cấu hình các thực thể Identity
        modelBuilder.Entity<IdentityRole>()
            .ToTable("Roles")
            .Property(x => x.Id).HasMaxLength(50).IsUnicode(false);

        modelBuilder.Entity<User>()
            .ToTable("Users")
            .Property(x => x.Id).HasMaxLength(50).IsUnicode(false);

        modelBuilder.Entity<IdentityRoleClaim<string>>()
            .ToTable("RoleClaims")
            .HasKey(x => x.Id);  // Đảm bảo rằng IdentityRoleClaim có khóa chính

        modelBuilder.Entity<IdentityUserClaim<string>>()
            .ToTable("UserClaims")
            .HasKey(x => x.Id);  // Đảm bảo rằng IdentityUserClaim có khóa chính

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .ToTable("UserLogins")
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });  // Định nghĩa khóa chính composite cho IdentityUserLogin

        modelBuilder.Entity<IdentityUserToken<string>>()
            .ToTable("UserTokens")
            .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });  // Đảm bảo rằng IdentityUserToken có khóa chính

        modelBuilder.Entity<IdentityUserRole<string>>()
            .ToTable("UserRoles")
            .HasKey(x => new { x.UserId, x.RoleId });  // Đảm bảo rằng IdentityUserRole có khóa chính composite

        // Áp dụng cấu hình tùy chỉnh cho các thực thể của bạn nếu cần thiết
        modelBuilder.ApplyConfiguration(new TasksConfiguration());
    }


    public DbSet<Tasks> Tasks { get; set; } = null!;

    public DbSet<Progress> Progress { get; set; } = null!;

    public DbSet<Documents> Documents { get; set; } = null!;

    public DbSet<Attachments> Attachments { get; set; } = null!;

}



