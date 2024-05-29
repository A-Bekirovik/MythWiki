using Microsoft.AspNetCore.Authentication.Cookies;
using MythWikiBusiness.Services;
using MythWikiData.Repository;
using MythWikiBusiness.IRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepo>(sp => new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ISubjectRepo>(sp => new SubjectRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SubjectService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();