using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.Helpers;

namespace Utility
{
    public class UtilityDependecy
    {

        public static void ALLDependency(IServiceCollection services)
        {
            services.AddSingleton(typeof(TaposRSA));
            
        }
    }
}
