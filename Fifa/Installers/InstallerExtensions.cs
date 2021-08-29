using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuraton) 
        {
            var ClassesImplementingInstaller = typeof(Startup).Assembly.ExportedTypes.Where(
        x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            ClassesImplementingInstaller.ForEach(installer => installer.InstallServices(services, configuraton));
        }
    }
}
