using System.Text.Json.Serialization;
using Heapify.Application.Common.Extensions;
using Heapify.Application.Users.Services;
using Heapify.Domain.Users.Entities;
using Heapify.Infrastructure.Common.Data;
using Heapify.Infrastructure.Common.Extensions;
using Heapify.Presentation.Auth.Filters;
using Heapify.Presentation.Auth.Routes;
using Heapify.Presentation.Projects.Routes;
using Heapify.Presentation.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace Heapify.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructureLayer(builder.Configuration)
            .AddApplicationLayer();

        builder.Services.AddProblemDetails();
        builder.Services.AddValidation();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();

        builder.Services.AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();

        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme);

        builder.Services.AddAuthorization();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;

            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddAntiforgery(options => { options.HeaderName = "X-XSRF-TOKEN"; });

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        if (builder.Environment.IsDevelopment())
        {
            var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");

            builder.Services.AddReverseProxy()
                .LoadFromConfig(reverseProxyConfig);
        }

        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        var api = app.MapGroup("api")
            .AddEndpointFilter<AntiforgeryEndpointFilter>()
            .RequireAuthorization();

        var v1 = api.MapGroup("v1");

        v1.MapAuthRoutes();
        v1.MapProjectRoutes();

        if (app.Environment.IsDevelopment())
        {
            app.MapReverseProxy();
        }
        else
        {
            app.MapFallbackToFile("index.html");
        }

        app.Run();
    }
}