// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Legacy;
using DragonFruit.Six.Api.Legacy.Entities;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Api.Seasonal.Entities;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Api.Services.Verification;
using DragonFruit.Six.Client.Database.Entities;
using DragonFruit.Six.Client.Database.Services;
using DragonFruit.Six.Client.Overlays.Search;
using Microsoft.AspNetCore.Components;
using Realms;
using SeasonInfo = DragonFruit.Six.Client.Database.Entities.SeasonInfo;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class Stats
    {
        private UbisoftAccount _account;
        private const byte ModernSeasonStart = 23;
        internal const int ModernStatsRange = 60;

        /// <summary>
        /// The date the modern stats were introduced. Used as a reference for last seen "a long time ago"
        /// </summary>
        private static DateTime ModernSeasonStartDate => new(2021, 09, 14);

        [Parameter]
        public string Identifier { get; set; }

        [Parameter]
        public Platform Platform { get; set; }

        [CascadingParameter]
        private SearchProviderState SearchProviderState { get; set; }

        [Inject]
        private Dragon6Client Client { get; set; }

        [Inject]
        private UserLookupCache UserCache { get; set; }

        [Inject]
        private AccountLookupCache AccountCache { get; set; }

        [Inject]
        private IMapper EntityMapper { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private Dragon6User User { get; set; }

        private UbisoftAccount Account
        {
            get => _account;
            set
            {
                _account = value;

                if (User?.ProfileId != Account?.ProfileId)
                {
                    User = null;
                    LastUpdated = null;
                }
            }
        }

        private bool? UserPlayedGame { get; set; }
        private DateTime? LastUpdated { get; set; }

        private LegacyPlaylistStats Casual { get; set; }
        private LegacyPlaylistStats Ranked { get; set; }
        private LegacyPlaylistStats Deathmatch { get; set; }

        /// <summary>
        /// Seasonal stats for the provided <see cref="Account"/> after the legacy stats were frozen.
        /// Used to recalculate all-time stats and quickly load in data to the season selector.
        /// </summary>
        private IReadOnlyCollection<SeasonalStats> PostFreezeSeasons { get; set; }

        protected override void OnParametersSet()
        {
            // reset all stats properties
            Account = null;
            UserPlayedGame = null;

            Casual = null;
            Ranked = null;
            Deathmatch = null;
        }

        protected override async Task OnParametersSetAsync()
        {
            // use search provider account if not directly loaded in
            if (string.IsNullOrEmpty(Identifier))
            {
                Account = SearchProviderState.DiscoveredAccount;
            }
            else
            {
                Account = await AccountCache.LookupAsync(Identifier, Platform, Guid.TryParse(Identifier, out _) ? IdentifierType.UserId : IdentifierType.Name).ConfigureAwait(false);
            }

            if (Account == null)
            {
                Navigation.NavigateTo("/home");
                return;
            }

            // do user lookup - may return either 0 or 1 results
            User = await UserCache.LookupAsync(Account.ProfileId, Platform, IdentifierType.UserId).ConfigureAwait(false) ?? new Dragon6User { ProfileId = Account.ProfileId, AccountRole = AccountRole.Normal };
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);

            if (User.AccountRole < AccountRole.Normal)
            {
                UserPlayedGame = false;
                return;
            }

            var legacyStats = await Client.GetLegacyStatsAsync(Account).ConfigureAwait(false);
            var deathmatch = new LegacyPlaylistStats();

            if (legacyStats == null)
            {
                UserPlayedGame = false;
                return;
            }

            UserPlayedGame = true;

            // get latest season id and create range to collect from the server
            using (var realm = await Realm.GetInstanceAsync().ConfigureAwait(true))
            {
                var latestSeason = Math.Max(ModernSeasonStart, realm.All<SeasonInfo>().OrderByDescending(x => x.SeasonId).First().SeasonId);
                var range = Enumerable.Range(ModernSeasonStart + 1, latestSeason - ModernSeasonStart);

                var postFreezeSeasonsEnumerable = await Client.GetSeasonalStatsRecordsAsync(Account, range.Append(-1), BoardType.Casual | BoardType.Ranked | BoardType.Deathmatch, Region.EMEA).ConfigureAwait(false);
                PostFreezeSeasons = postFreezeSeasonsEnumerable.ToList();
            }

            // the ubisoft activity api has been frozen so use leaderboards to work out the last time the user played a game.
            var lastLeaderboardUpdate = PostFreezeSeasons.Max(x => x.TimeUpdated);
            LastUpdated = lastLeaderboardUpdate > ModernSeasonStartDate ? lastLeaderboardUpdate : null;

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

            // add/update user recent profiles
            using (var realm = await Realm.GetInstanceAsync())
            {
                var recentAccount = EntityMapper.Map<RecentAccount>(Account);
                await realm.WriteAsync(() => realm.Add(recentAccount, true)).ConfigureAwait(false);
            }
        }
    }
}
