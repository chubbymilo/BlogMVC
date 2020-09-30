using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMVC.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                

                var scope = host.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userM = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();
                var adminRole = new IdentityRole("Admin");
                if (!context.Roles.Any())
                {
                    roleM.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!context.Users.Any())
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "414052254@qq.com",
                        Email = "414052254@qq.com",
                        EmailConfirmed = true,


                    };

                    userM.CreateAsync(adminUser, "zhaohaiyu").GetAwaiter().GetResult();
                    userM.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
