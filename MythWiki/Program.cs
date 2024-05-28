using Microsoft.AspNetCore.Authentication.Cookies;
using MythWikiBusiness.Services;
using MythWikiData.Repository;
using MythWikiBusiness.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure dependency injection for IUserRepo and UserService
builder.Services.AddScoped<IUserRepo>(sp => new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ISubjectRepo>(sp => new SubjectRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SubjectService>();

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();