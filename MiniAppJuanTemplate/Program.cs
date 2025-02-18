using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate;
using MiniAppJuanTemplate.Areas.Manage.Services.Implements;
using MiniAppJuanTemplate.Areas.Manage.Services.Interfaces;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Register(config);
// Add services to the container.

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
  name: "areas",
  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
