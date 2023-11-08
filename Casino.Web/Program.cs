using Casino.Application.Abstraction;
using Casino.Application.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMemberAdminService, MemberAdminDbFakeService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IGameAdminService, GameAdminDbFakeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "MyAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Member}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "MyAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Game}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
