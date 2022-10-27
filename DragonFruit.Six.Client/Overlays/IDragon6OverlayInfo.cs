// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays
{
    public interface IDragon6OverlayInfo
    {
        int Order { get; }

        string Name { get; }
        string FullName { get; }

        string Icon { get; }

        OffcanvasSize Size { get; }
        Type OverlayContent { get; }
    }
}
