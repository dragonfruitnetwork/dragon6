// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Network
{
    public interface IDragon6ServiceInjector
    {
        /// <summary>
        /// Component responsible for injecting a set of services into the DI container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to inject dependencies into</param>
        public void InitialiseServices(IServiceCollection services);
    }
}
