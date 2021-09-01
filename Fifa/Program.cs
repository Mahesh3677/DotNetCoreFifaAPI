using Fifa.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
          var host =  CreateWebHostBuilder(args).Build();

            using (var servicescope = host.Services.CreateScope())
            {
                //var dbcontext = servicescope.ServiceProvider.GetRequiredService<DataContext>();

                //await dbcontext.Database.migr

                var rolemanager = servicescope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if(!await rolemanager.RoleExistsAsync("Admin"))
                {
                    var adminrole = new IdentityRole("Admin");
                    await rolemanager.CreateAsync(adminrole);
                }

                if (!await rolemanager.RoleExistsAsync("Poster"))
                {
                    var adminrole = new IdentityRole("Poster");
                    await rolemanager.CreateAsync(adminrole);
                }
            }

                await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
