using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Practica2.Contexts;
using Practica2.Mapper;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Context>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EntityMapper>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => { x.LoginPath = "/Login/Login"; x.AccessDeniedPath = new PathString("/Unauthorized");
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = false;
    x.AccessDeniedPath = "/Forbidden/";

});

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
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}");

app.Run();
