// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Data.Basic;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Database.Entities;
using DragonFruit.Six.Client.Overlays.Recents;
using Microsoft.AspNetCore.Components;
using Realms;

namespace DragonFruit.Six.Client.Screens.Splash
{
    public partial class Splash
    {
        [Inject]
        private IDragon6Platform Platform { get; set; }

        [Inject]
        private IServiceProvider Services { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private Dragon6Configuration Configuration { get; set; }

        private string CurrentStatus { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentStatus = "Loading Configuration...";
            Configuration.SetDefaults();

            RealmConfigurator.Initialise(Platform);

            CurrentStatus = "Preparing database...";
            await StaticAssetUpdater.UpdateTable<SeasonInfo>(Services, () => new BasicApiRequest("https://d6static.dragonfruit.network/data/seasons.json")).ConfigureAwait(false);
            await StaticAssetUpdater.UpdateTable<OperatorInfo>(Services, () => new BasicApiRequest("https://d6static.dragonfruit.network/data/operators-v2.json")).ConfigureAwait(false);

            using (var realm = await Realm.GetInstanceAsync())
            {
                if (realm.All<RecentAccount>().Count() > RecentsOverlayInfo.MaxAccounts)
                {
                    // todo move purge logic to reflection-based system
                    // because realm can't do Skip() inside a queryable, get the first expired account search date and use that to determine which ones need removing.
                    var recentAccountMinSearchDate = realm.All<RecentAccount>().OrderByDescending(x => x.LastSearched).AsEnumerable().Skip(RecentsOverlayInfo.MaxAccounts).FirstOrDefault()?.LastSearched;
                    await realm.WriteAsync(() => realm.RemoveRange(realm.All<RecentAccount>().Where(x => x.LastSearched < recentAccountMinSearchDate)));
                }
            }

            // reduce filesize
            Realm.Compact();

            CurrentStatus = "Welcome to Dragon6";
            await Task.Delay(1000).ConfigureAwait(false);

            Navigation.NavigateTo("/home");
        }
    }
}
