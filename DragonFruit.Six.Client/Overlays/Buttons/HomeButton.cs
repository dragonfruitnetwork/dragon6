// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using Microsoft.AspNetCore.Components;

namespace DragonFruit.Six.Client.Overlays.Buttons
{
    public class HomeButton : INavigationButton
    {
        public int Order => 0;
        public string Icon => "home";
        public string Name => "Home";

        public void OnClick(NavigationManager nav) => nav.NavigateTo("/home");
    }
}
