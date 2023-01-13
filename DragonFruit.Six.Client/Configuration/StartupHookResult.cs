// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Collections.Generic;
using DragonFruit.Six.Client.Overlays;

namespace DragonFruit.Six.Client.Configuration
{
    /// <summary>
    /// Represents the result of an <see cref="IStartupHook"/> result
    /// </summary>
    public class StartupHookResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public IEnumerable<IServiceButton> Buttons { get; set; }
    }
}
