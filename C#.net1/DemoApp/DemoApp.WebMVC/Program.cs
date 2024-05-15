using DemoApp.Domain.Abstractions;
using DemoApp.Persistence;
using DemoApp.Application;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
	.AddRazorRuntimeCompilation();

builder.Services.AddRepositoryUnitOfWork();
builder.Services.AddEfCore(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(1);
	options.Cookie.HttpOnly = true;
});

builder.Services.AddCustomIdentity();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(option =>
				{
					option.LoginPath = "/account/login";
					option.AccessDeniedPath = "/Account/AccessDenied";
					option.Cookie.HttpOnly = true;
					option.Cookie.Expiration = TimeSpan.FromHours(1);
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
//use middleware for session
app.UseSession();

app.UseRouting();

//Authen luon dung truoc author
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
