using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Services;
using Microsoft.OpenApi.Models;
using TasksManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using TasksManager.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TasksDbContext") ?? throw new InvalidOperationException("Connection string 'TasksDbContext' not found.")));

//Setup identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TasksDbContext>()
    .AddDefaultTokenProviders();

//automapper config
builder.Services.AddAutoMapper(typeof(Program));

//DI configuration
builder.Services.AddTransient<ITasksService, TasksService>();
builder.Services.AddTransient<IAttachmentService, AttachmentService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IProgressService, ProgressService>();
builder.Services.AddTransient<IUsersService, UsersService>();

//File Storage
builder.Services.AddTransient<IStorageService, FileStorageService>();

//DbInitializer
builder.Services.AddTransient<DbInitializer>();

//Jwt Authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));


// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Task Management", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var jwtOptions = builder.Configuration
    .GetSection("JwtOptions")
    .Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions!);

//Configuring the Authentication Service
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(opts =>
{
    //convert the string signing key to byte array

    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions!.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey((byte[]?)Encoding.UTF8
        .GetBytes(jwtOptions!.SigningKey!))
    };
});


// Add session support
// Add memory cache
builder.Services.AddDistributedMemoryCache();

// Add session support 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//
app.UseSession();  // Phải đặt trước UseRouting
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Task Management V1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
    db.Database.Migrate();
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Seeding data...");
        var dbInitializer = serviceProvider.GetService<DbInitializer>();
        if (dbInitializer != null)
            dbInitializer.Seed()
                         .Wait();
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
