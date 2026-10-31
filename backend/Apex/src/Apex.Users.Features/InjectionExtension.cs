using Apex.Users.Domain.Entities;
using Apex.Users.Features.Modules.Auth;
using Apex.Users.Features.Modules.Auth.Models;
using Apex.Users.Features.Modules.Users.Details.Models;
using Apex.Users.Features.Modules.Users.Details.Services;
using Apex.Users.Features.Modules.Users.List.Models;
using Apex.Users.Features.Modules.Users.List.Services;
using Apex.Shared.Core.Services;
using Apex.Users.Features.Security;
using Apex.Users.Persistance;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Users.Features;

public static class InjectionExtension
{
    public static void AddFeaturesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection("Authentication"));

        services.AddHttpContextAccessor();

        services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddRoles<UserRole>()
            .AddRoleManager<RoleManager<UserRole>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<UsersDbContext>();

        services.AddScoped<IValidationRunner, ValidationRunner>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<AuthService>();
        services.AddScoped<IAuthService>(sp =>
            new AuthDecorator(sp.GetRequiredService<AuthService>(), sp.GetRequiredService<IValidationRunner>()));

        RegisterMappers(services);
        RegisterUsers(services);
    }

    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddSingleton<RegisterModelMapper>();
        services.AddSingleton<UserModelMapper>();
        services.AddSingleton<UserListFilterMapper>();
        services.AddSingleton<UserListModelMapper>();
    }

    private static void RegisterUsers(IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<IUserService>(sp =>
            new UserDecorator(sp.GetRequiredService<UserService>(), sp.GetRequiredService<IValidationRunner>()));

        services.AddScoped<UserListService>();
        services.AddScoped<IUserListService>(sp =>
            new UserListDecorator(sp.GetRequiredService<UserListService>(), sp.GetRequiredService<IValidationRunner>()));
    }
}
