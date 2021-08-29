using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Fifa.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection serviceCollection, IConfiguration configuraton);
    }
}
