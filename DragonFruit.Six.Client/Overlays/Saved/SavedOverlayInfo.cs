// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays.Saved
{
    public class SavedOverlayInfo : IDragon6OverlayInfo
    {
        public int Order => 1;

        public string Name => "Saved";
        public string FullName => "Saved Players";

        public string Icon => "star";

        public OffcanvasSize Size => OffcanvasSize.Large;
        public Type OverlayContent => typeof(SavedOverlay);
    }
}
