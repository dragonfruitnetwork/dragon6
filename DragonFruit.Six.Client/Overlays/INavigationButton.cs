// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.AspNetCore.Components;

namespace DragonFruit.Six.Client.Overlays
{
    /// <summary>
    /// Represents a button that has a callback that requires access to the context's <see cref="NavigationManager"/>
    /// </summary>
    public interface INavigationButton : IHeaderButton
    {
        void OnClick(NavigationManager nav);
    }
}
