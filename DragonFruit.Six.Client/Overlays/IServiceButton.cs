// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;

namespace DragonFruit.Six.Client.Overlays
{
    /// <summary>
    /// Represents a button that has a callback that requires access to the context's <see cref="IServiceProvider"/> to perform its purpose
    /// </summary>
    public interface IServiceButton : IHeaderButton
    {
        string Icon { get; }
        string Name { get; }

        void OnClick(IServiceProvider services);
    }
}
