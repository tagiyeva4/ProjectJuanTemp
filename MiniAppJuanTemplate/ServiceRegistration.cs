using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Areas.Manage.Services.Implements;
using MiniAppJuanTemplate.Areas.Manage.Services.Interfaces;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;
using MiniAppJuanTemplate.Services;

namespace MiniAppJuanTemplate
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services,IConfiguration config)
        {

            services.AddControllersWithViews();

            services.AddDbContext<JuanAppDbContext>(options =>
            {
                options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
            });
            services.AddHttpContextAccessor();

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<LayoutServices>();
            services.AddScoped<EmailService>();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "748514526525-qpbosdvbv58ivo0a1cklh8n2826sqglu.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                    options.SaveTokens = true;
                });
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<JuanAppDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
                {
                    var uri = new Uri(context.RedirectUri);
                    if (context.Request.Path.Value.ToLower().StartsWith("/manage"))
                    {
                        context.Response.Redirect("/manage/account/login" + uri.Query);
                    }
                    else
                    {
                        context.Response.Redirect("/account/login" + uri.Query);

                    }
                    return Task.CompletedTask;
                };

            });
        }
    }
}
