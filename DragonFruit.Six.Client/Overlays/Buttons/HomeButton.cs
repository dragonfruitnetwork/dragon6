// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Overlays.Buttons
{
    public class HomeButton : IServiceButton
    {
        public int Order => 0;
        public int ColumnSize => 12;

        public string Icon => "home";
        public string Name => "Home";

        public void OnClick(IServiceProvider services) => services.GetRequiredService<NavigationManager>().NavigateTo("/home");
    }
}
