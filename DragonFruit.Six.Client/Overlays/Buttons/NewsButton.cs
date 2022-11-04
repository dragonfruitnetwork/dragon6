// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Overlays.Buttons
{
    public class NewsButton : IServiceButton
    {
        public int Order => 3;
        public int ColumnSize => 4;

        public string Icon => "compass";
        public string Name => "News";

        public void OnClick(IServiceProvider services) => services.GetRequiredService<NavigationManager>().NavigateTo("/news");
    }
}
