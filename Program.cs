using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TasksDbContext") ?? throw new InvalidOperationException("Connection string 'TasksDbContext' not found.")));

//automapper config
builder.Services.AddAutoMapper(typeof(Program));

//DI configuration
builder.Services.AddTransient<ITasksService, TasksService>();
builder.Services.AddTransient<IAttachmentService, AttachmentService>();
//builder.Services.AddTransient<ILessonsService, LessonsService>();
//builder.Services.AddTransient<ILessonsService, LessonsService>();

//File Storage
builder.Services.AddTransient<IStorageService, FileStorageService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DbInitializer.Seed(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
