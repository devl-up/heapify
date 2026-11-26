using System.Net;
using System.Text.Json.Serialization;
using Heapify.Application.Common.Extensions;
using Heapify.Application.Users.Services;
using Heapify.Domain.Users.Entities;
using Heapify.Infrastructure.Common.Data;
using Heapify.Infrastructure.Common.Extensions;
using Heapify.Presentation.Common.AuthorizationHandlers;
using Heapify.Presentation.Common.Extensions;
using Heapify.Presentation.Common.Filters;
using Heapify.Presentation.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Heapify.Presentation;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi(o =>
        {
            var defaultSchemaReferenceId = o.CreateSchemaReferenceId;

            o.CreateSchemaReferenceId = t =>
            {
                if (t.Type.FullName == null || !t.Type.FullName.StartsWith("Heapify"))
                {
                    return defaultSchemaReferenceId(t);
                }

                return GetSchemaName(t.Type);

                static string GetSchemaName(Type type)
                {
                    if (type.IsArray)
                    {
                        return $"{GetSchemaName(type.GetElementType()!)}Array{type.GetArrayRank()}";
                    }

                    var name = type.Name.Split('`')[0]
                        .Replace("Endpoint", string.Empty);

                    if (type.DeclaringType is { } declaringType)
                    {
                        name = $"{GetSchemaName(declaringType)}.{name}";
                    }

                    if (type.IsGenericType)
                    {
                        name += $"Of{string.Join("And", type.GetGenericArguments().Select(GetSchemaName))}";
                    }

                    return name;
                }
            };
        });

        builder.Services.AddInfrastructureLayer(builder.Configuration)
            .AddApplicationLayer();

        builder.Services.AddProblemDetails();
        builder.Services.AddValidation();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IAuthorizationHandler, ProjectPermissionAuthorizationHandler>();

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

        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
        }

        app.UseHttpsRedirection();

        if (!app.Environment.IsDevelopment())
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        var api = app.MapGroup("api")
            .AddEndpointFilter<AntiforgeryEndpointFilter>()
            .RequireAuthorization();

        var v1 = api.MapGroup("v1");

        v1.MapEndpoints();

        api.ProducesProblem((int)HttpStatusCode.InternalServerError);

        if (!app.Environment.IsDevelopment())
        {
            app.MapFallback("/api/{**path}", () => Results.NotFound());
            app.MapFallbackToFile("index.html");
        }

        app.Run();
    }
}