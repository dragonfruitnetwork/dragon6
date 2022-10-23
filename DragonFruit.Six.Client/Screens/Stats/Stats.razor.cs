// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Legacy;
using DragonFruit.Six.Api.Legacy.Entities;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Api.Seasonal.Entities;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Client.Overlays.Search;
using Microsoft.AspNetCore.Components;
using Realms;
using SeasonInfo = DragonFruit.Six.Client.Database.Entities.SeasonInfo;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class Stats
    {
        private const int ModernSeasonStart = 23;

        [Parameter]
        public string Identifier { get; set; }

        [Parameter]
        public Platform Platform { get; set; }

        [CascadingParameter]
        private SearchProviderState SearchProviderState { get; set; }

        [Inject]
        private Dragon6Client Client { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private UbisoftAccount Account { get; set; }
        private UbisoftAccountActivity AccountActivity { get; set; }

        private LegacyPlaylistStats Casual { get; set; }
        private LegacyPlaylistStats Ranked { get; set; }
        private LegacyPlaylistStats Deathmatch { get; set; }

        /// <summary>
        /// Seasonal stats for the provided <see cref="Account"/> after the legacy stats were frozen.
        /// Used to recalculate all-time stats and quickly load in data to the season selector.
        /// </summary>
        private IEnumerable<SeasonalStats> PostFreezeSeasons { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            // use search provider account if not directly loaded in
            if (string.IsNullOrEmpty(Identifier))
            {
                Account = SearchProviderState.DiscoveredAccount;
            }
            else
            {
                Account = await Client.GetAccountAsync(Identifier, Platform, Guid.TryParse(Identifier, out _) ? IdentifierType.UserId : IdentifierType.Name).ConfigureAwait(false);
            }

            if (Account == null)
            {
                Navigation.NavigateTo("/home");
            }

            AccountActivity = await Client.GetAccountActivityAsync(Account).ConfigureAwait(false);

            if (AccountActivity == null || AccountActivity.SessionCount == 0)
            {
                return;
            }

            var legacyStats = await Client.GetLegacyStatsAsync(Account).ConfigureAwait(false);
            var deathmatch = new LegacyPlaylistStats();

            // get latest season id and create range to collect from the server
            using (var realm = await Realm.GetInstanceAsync().ConfigureAwait(true))
            {
                var latestSeason = Math.Max(ModernSeasonStart, realm.All<SeasonInfo>().OrderByDescending(x => x.SeasonId).First().SeasonId);
                var range = Enumerable.Range(ModernSeasonStart + 1, latestSeason - ModernSeasonStart);

                PostFreezeSeasons = await Client.GetSeasonalStatsRecordsAsync(Account, range.Append(-1), BoardType.Casual | BoardType.Ranked | BoardType.Deathmatch, Region.EMEA).ConfigureAwait(false);
            }

            // take all post-freeze seasons and add the stats together
            // all deathmatch stats will always be included in post-freeze, because deathmatch was introduced in season 25
            foreach (var season in PostFreezeSeasons)
            {
                var targetPlaylist = season.Board switch
                {
                    BoardType.Ranked => legacyStats.Ranked,
                    BoardType.Casual => legacyStats.Casual,
                    BoardType.Deathmatch => deathmatch,

                    _ => throw new ArgumentOutOfRangeException()
                };

                targetPlaylist.Include(season);
            }

            Casual = legacyStats.Casual;
            Ranked = legacyStats.Ranked;
            Deathmatch = deathmatch;
        }
    }
}
