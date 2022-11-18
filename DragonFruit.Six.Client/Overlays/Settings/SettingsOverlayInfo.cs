// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Havit.Blazor.Components.Web.Bootstrap;

namespace DragonFruit.Six.Client.Overlays.Settings
{
    public class SettingsOverlayInfo : IOverlayInfo
    {
        public int Order => 5;
        public int ColumnSize => 4;

        public string Icon => "gear";
        public string Name => "Settings";
        public string FullName => "Settings";

        public OffcanvasSize Size => OffcanvasSize.Large;
        public Type OverlayContent => typeof(SettingsOverlay);
    }
}
