// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Collections.Generic;
using System.Threading.Tasks;
using DragonFruit.Six.Client.Overlays;

namespace DragonFruit.Six.Client.Configuration
{
    /// <summary>
    /// Represents the result of an <see cref="IStartupHook"/> result
    /// </summary>
    public class StartupHookResult
    {
        /// <summary>
        /// Whether the normal process flow should be interrupted
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message to display if <see cref="Success"/> is <c>false<c/>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional <see cref="Task"/> that will block the flow from completing.
        /// Can be used to provide some form of extended loading or confirmation prompts
        /// </summary>
        public Task FlowContinuationTask { get; set; }

        /// <summary>
        /// Buttons to display.
        /// </summary>
        public IEnumerable<IServiceButton> Buttons { get; set; }
    }
}
