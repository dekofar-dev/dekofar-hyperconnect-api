using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Extensions
{
    public static class NetGsmServiceCollectionExtensions
    {
        public static IServiceCollection AddNetGsmIntegration(this IServiceCollection services)
        {
            services.AddScoped<INetGsmCallService, NetGsmCallService>();
            return services;
        }
    }
}
