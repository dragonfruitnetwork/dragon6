// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays.Recents
{
    public class RecentsOverlayInfo : IDragon6OverlayInfo
    {
        public int Order => 2;

        public string Name => "Recents";
        public string FullName => "Recent Players";

        public string Icon => "clock-rotate-left";

        public OffcanvasSize Size => OffcanvasSize.Large;
        public Type OverlayContent => typeof(RecentsOverlay);
    }
}
