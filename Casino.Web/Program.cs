using Casino.Application.Abstraction;
using Casino.Application.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Casino.Domain.Identity;
using Casino.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Create a new web application builder with the provided command line arguments
var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers and views to the service collection
builder.Services.AddControllersWithViews();

// Register the FileUploadService with scoped lifetime and dependency on IWebHostEnvironment
builder.Services.AddScoped<IFileUploadService, FileUploadService>(serviceProvider =>
    new FileUploadService(serviceProvider.GetService<IWebHostEnvironment>().WebRootPath));

// Register other services with scoped lifetime
builder.Services.AddScoped<IGameAdminService, GameAdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IAccountService, AccountIdentityService>();
builder.Services.AddScoped<IDepositService, DepositService>();

// Configure and register the database context for the application using SQL Server
builder.Services.AddDbContext<CasinoDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Configure and register the Identity framework services
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<CasinoDbContext>()
    .AddDefaultTokenProviders();

// Configure the IdentityOptions for the application
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password and lockout settings
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.User.RequireUniqueEmail = true;
});

// Configure the application cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings for authentication
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Security/Account/Login";
    options.LogoutPath = "/Security/Account/Logout";
    options.SlidingExpiration = true;
});

// Build the application
var app = builder.Build();

// Configure the app's HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // Use exception handler and HSTS in production environment
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware for HTTPS redirection, serving static files, routing, authentication, and authorization
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Define routes for areas and default controller actions
app.MapAreaControllerRoute(
    name: "MyAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=User}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "MyAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Game}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=CoinFlip}/{action=FlipCoin}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();