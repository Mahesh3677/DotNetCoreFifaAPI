using Fifa.Data;
using Fifa.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuraton)
        {
            services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(
                  configuraton.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()

                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IPostService, PostService>();
        }
    }
}
