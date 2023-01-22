// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Client.Overlays.Search;
using DragonFruit.Six.Client.Presence;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Screens.Home
{
    public partial class Home
    {
        [Inject]
        private SearchProviderState SearchProvider { get; set; }

        [Inject]
        private IServiceProvider Services { get; set; }

        protected override void OnInitialized()
        {
            Services.GetService<IPresenceClient>()?.PushUpdate(new PresenceStatus("Homepage"));
        }
    }
}
