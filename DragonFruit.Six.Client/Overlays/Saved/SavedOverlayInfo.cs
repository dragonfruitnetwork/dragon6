// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays.Saved
{
    public class SavedOverlayInfo : IOverlayInfo
    {
        public int Order => 1;
        public string Icon => "star";
        public string Name => "Saved";

        public string FullName => "Saved Players";
        public OffcanvasSize Size => OffcanvasSize.Large;
        public Type OverlayContent => typeof(SavedOverlay);
    }
}
