using Fifa.Data;
using Fifa.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //using (var servicescope = host.Services.CreateScope())
            //{
            //    var dbcontext = servicescope.ServiceProvider.GetRequiredService<DataContext>();

            //    //await dbcontext.Database.migr

            //    //var rolemanager = servicescope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    //if(!await rolemanager.RoleExistsAsync("Admin"))
            //    //{
            //    //    var adminrole = new IdentityRole("Admin");
            //    //    await rolemanager.CreateAsync(adminrole);
            //    //}

            //    //if (!await rolemanager.RoleExistsAsync("Poster"))
            //    //{
            //    //    var adminrole = new IdentityRole("Poster");
            //    //    await rolemanager.CreateAsync(adminrole);
            //    //}

                
            //    //Stopwatch sw = new Stopwatch();
            //    //sw.Start();
            //    ////for (int i = 0; i < 1000;i++)
            //    ////{
            //    ////    Country c = new Country
            //    ////    {
            //    ////        CountryId = Guid.NewGuid(),
            //    ////        Name = "club",
            //    ////        FlagImage = "image"
            //    ////    };
            //    ////    await dbcontext.Country.AddAsync(c);
            //    ////    await dbcontext.SaveChangesAsync();
            //    ////}
            //    //List<Country> cl = await dbcontext.Country.ToListAsync();
            //    //sw.Stop();
            

            //    //Stopwatch swsq = new Stopwatch();
            //    //swsq.Start();
            //    ////for (int i = 0; i < 1000; i++)
            //    ////{
            //    ////    Country c = new Country
            //    ////    {
            //    ////        CountryId =Guid.NewGuid(),
            //    ////        Name = "club",
            //    ////        FlagImage = "image"
            //    ////    };
            //    ////    var countryId = new SqlParameter("@CountryId", c.CountryId);
            //    ////    var name = new SqlParameter("@Name", c.Name);
            //    ////    var flagImage = new SqlParameter("@FlagImage", c.FlagImage);
            //    ////    var data = await dbcontext.Database.ExecuteSqlRawAsync("CountryInsert @CountryId, @Name , @FlagImage",countryId, name, flagImage);
            //    ////}
            //    //IQueryable<Country> clsp =  dbcontext.Country.FromSqlRaw("Select * from dbo.country");
            //    //sw.Stop();
            //    //Console.WriteLine("EF -" + sw.ElapsedMilliseconds.ToString());
            //    //Console.WriteLine("procedure -" + swsq.ElapsedMilliseconds.ToString());
            //    //Console.ReadLine();
            //}

                await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
