using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace TasksManager.Data
{
    public class DbInitializer
    {
        private readonly TasksDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string AdminRoleName = "Admin";
        private readonly string UserRoleName = "Member";

        public DbInitializer(TasksDbContext context,
          UserManager<User> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            // Seeding role
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = UserRoleName,
                    NormalizedName = UserRoleName.ToUpper(),
                });
            }

            // Seeding user
            if (!_userManager.Users.Any())
            {
                var result = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "example@gmail.com",
                    FullName = "Example",
                    Email = "example@gmail.com",
                    LockoutEnabled = false,
                    PhoneNumber = "0987654321",
                    Dob = new DateTime(2000, 01, 01)
                }, "Admin@123");
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("example@gmail.com");
                    if (user != null)
                        await _userManager.AddToRoleAsync(user, AdminRoleName);
                }
            }

        }
    }
}
