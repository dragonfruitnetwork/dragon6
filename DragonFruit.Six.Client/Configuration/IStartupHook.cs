// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;

namespace DragonFruit.Six.Client.Configuration
{
    public interface IStartupHook
    {
        public Task<StartupHookResult> ValidateOnStartup(IServiceProvider services);
    }
}
