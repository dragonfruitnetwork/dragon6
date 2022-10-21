// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Api;
using DragonFruit.Six.Client.Network;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Interop
{
    public class Dragon6DeveloperServices : IDragon6Services
    {
        public void InitialiseServices(IServiceCollection services)
        {
            services.AddSingleton<Dragon6Client, Dragon6DebugClient>();
        }
    }
}
