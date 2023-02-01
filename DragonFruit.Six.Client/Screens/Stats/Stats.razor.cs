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
using DragonFruit.Six.Client.Database.Entities;
using DragonFruit.Six.Client.Database.Services;
using DragonFruit.Six.Client.Network.User;
using DragonFruit.Six.Client.Overlays.Search;
using DragonFruit.Six.Client.Presence;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using SeasonInfo = DragonFruit.Six.Client.Database.Entities.SeasonInfo;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class Stats : IDisposable
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

        [Inject]
        private SearchProviderState SearchProviderState { get; set; }

        [Inject]
        private Dragon6Client Client { get; set; }

        [Inject]
        private UserLookupCache UserCache { get; set; }

        [Inject]
        private AccountLookupCache AccountCache { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private IServiceProvider Services { get; set; }

        private IDragon6User User { get; set; }

        private UbisoftAccount Account
        {
            get => _account;
            set
            {
                _account = value;

                if (User?.UbisoftId != Account?.UbisoftId)
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
        private LegacyPlaylistStats Warmup { get; set; }

        /// <summary>
        /// Seasonal stats for the provided <see cref="Account"/> after the legacy stats were frozen.
        /// Used to recalculate all-time stats and quickly load in data to the season selector.
        /// </summary>
        private IReadOnlyCollection<SeasonalStats> PostFreezeSeasons { get; set; }

        protected override void OnInitialized()
        {
            SearchProviderState.AccountLoaded += OnAccountLoaded;
            Services.GetService<IPresenceClient>()?.PushUpdate(new PresenceStatus("Stats"));
        }

        private void OnAccountLoaded(UbisoftAccount obj) => UpdateStats(true);
        protected override Task OnParametersSetAsync() => UpdateStats(false);

        private async Task UpdateStats(bool eventTriggered)
        {
            // reset all stats properties
            Account = null;
            UserPlayedGame = null;

            Casual = null;
            Ranked = null;
            Warmup = null;

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
            User = await UserCache.LookupAsync(Account.UbisoftId, Platform, IdentifierType.UserId).ConfigureAwait(false) ?? new Dragon6User { UbisoftId = Account.ProfileId, AccountRole = AccountRole.Normal };
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);

            if (User.AccountRole < AccountRole.Normal)
            {
                UserPlayedGame = false;
                return;
            }

            // in some cases, the user may not have "legacy" stats - potentially due to only playing deathmatch or newcomer.
            // when this happens, create a container with the properties needed for the process to finish.
            var warmup = new LegacyPlaylistStats();
            var legacyStats = await Client.GetLegacyStatsAsync(Account).ConfigureAwait(false) ?? new LegacyStats
            {
                Casual = new LegacyPlaylistStats(),
                Ranked = new LegacyPlaylistStats()
            };

            // get latest season id and create range to collect from the server
            IEnumerable<int> seasonIds;

            using (var realm = await Realm.GetInstanceAsync())
            {
                var latestSeason = Math.Max(ModernSeasonStart, realm.All<SeasonInfo>().OrderByDescending(x => x.SeasonId).First().SeasonId);
                seasonIds = Enumerable.Range(ModernSeasonStart + 1, latestSeason - ModernSeasonStart);
            }

            var postFreezeSeasonsEnumerable = await Client.GetSeasonalStatsRecordsAsync(Account, seasonIds.Append(-1), BoardType.Casual | BoardType.Ranked | BoardType.Warmup, Region.EMEA).ConfigureAwait(false);
            PostFreezeSeasons = postFreezeSeasonsEnumerable.ToList();

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
                    BoardType.Warmup => warmup,

                    _ => throw new ArgumentOutOfRangeException()
                };

                targetPlaylist.Include(season);
            }

            Casual = legacyStats.Casual;
            Ranked = legacyStats.Ranked;
            Warmup = warmup;

            // if a user only plays newcomer they will show up as not playing - nice for those that start off dying constantly
            UserPlayedGame = Casual.MatchesPlayed + Ranked.MatchesPlayed + Warmup.MatchesPlayed > 0;

            // add/update user recent profiles
            var recentAccount = Services.GetRequiredService<IMapper>().Map<RecentAccount>(Account);

            using (var realm = await Realm.GetInstanceAsync())
            {
                await realm.WriteAsync(() => realm.Add(recentAccount, true)).ConfigureAwait(false);
            }

            // event-based updates (i.e. searching for a user from within the stats page) requires a forced-update
            if (eventTriggered)
            {
                InvokeAsync(StateHasChanged);
            }
        }

        public void Dispose()
        {
            SearchProviderState.AccountLoaded -= OnAccountLoaded;
        }
    }
}
