﻿using Microsoft.AspNetCore.Identity;
using Quizle.DB.Models;
using System.Diagnostics.CodeAnalysis;

namespace Quizle.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions 
    {
        public static IApplicationBuilder SeedAdmin(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    IdentityRole role = new IdentityRole("Administrator");
                    await roleManager.CreateAsync(role);
                }

                ApplicationUser? admin = await userManager.FindByEmailAsync("administrator@gmail.com");
                if (admin != null)
                {
                    await userManager.AddToRoleAsync(admin, "Administrator");
                }
            })
            .GetAwaiter()
            .GetResult();
            return app;
        }
    }
}
