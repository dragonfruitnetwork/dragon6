// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Threading.Tasks;
using DragonFruit.Data.Basic;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Database.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Realms;

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

            CurrentStatus = "Updating database...";
            await StaticAssetUpdater.UpdateTable<SeasonInfo>(Services, () => new BasicApiRequest("https://d6static.dragonfruit.network/data/seasons.json")).ConfigureAwait(false);
            await StaticAssetUpdater.UpdateTable<OperatorInfo>(Services, () => new BasicApiRequest("https://d6static.dragonfruit.network/data/operators-v2.json")).ConfigureAwait(false);

            // reduce filesize
            Realm.Compact();

            CurrentStatus = "Welcome to Dragon6";
            await Task.Delay(500).ConfigureAwait(false);

            Navigation.NavigateTo("/home");
        }
    }
}
