// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;
using DragonFruit.Six.Api.Seasonal.Requests;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Database.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFruit.Six.Client.Screens
{
    public partial class Splash
    {
        [Inject]
        private IServiceProvider Services { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private string CurrentStatus { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RealmConfigurator.Initialise(Services.GetRequiredService<IFileSystemStructure>());

            CurrentStatus = "Setting up database...";
            await StaticAssetUpdater.UpdateTable<SeasonInfo>(Services, () => new SeasonInfoRequest()).ConfigureAwait(false);

            CurrentStatus = "Welcome to Dragon6";
            await Task.Delay(500).ConfigureAwait(false);

            Navigation.NavigateTo("/home");
        }
    }
}
