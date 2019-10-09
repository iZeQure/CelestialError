using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevNet.Interfaces
{
    interface IConfigurationService
    {
        ServiceProvider ConfigurationServices();
    }
}
