// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Enums;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Api.Seasonal.Entities;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Realms;
using SeasonInfo = DragonFruit.Six.Client.Database.Entities.SeasonInfo;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class StatsSeasonal
    {
        private const int EntriesPerPage = 4;

        [Inject]
        public Dragon6Client Client { get; set; }

        [Inject]
        public Dragon6Configuration Configuration { get; set; }

        [Parameter]
        public UbisoftAccount Account { get; set; }

        [Parameter]
        public IEnumerable<SeasonalStats> PreloadedStats { get; set; }

        private BoardType SelectedBoard { get; set; } = BoardType.Ranked;
        private SeasonalStatsContainer SelectedSeason { get; set; }
        private ILookup<BoardType, SeasonalStatsContainer> Boards { get; set; }

        private int Page { get; set; }
        private int MaxPage => (Boards[SelectedBoard].Count() - 1) / EntriesPerPage + 1;

        protected override void OnParametersSet()
        {
            Boards = null;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (PreloadedStats?.Any() != true)
                return;

            using var realm = await Realm.GetInstanceAsync();
            var preloadedSeasons = PreloadedStats.Join(realm.All<SeasonInfo>(), x => x.SeasonId, x => x.SeasonId, (s, i) => new SeasonalStatsContainer(i.Freeze(), s))
                                                 .OrderByDescending(x => x.Info.SeasonId)
                                                 .ToList();

            Page = 1;
            Boards = preloadedSeasons.ToLookup(x => x.Stats.Board);

            _ = InvokeAsync(StateHasChanged);

            // because realm can't process Contains(), we need to use the smallest season id we retrieved and go backwards from there.
            var lastPrefetchedSeason = PreloadedStats.Min(y => y.SeasonId);
            var missingSeasonInfo = realm.All<SeasonInfo>()
                                         .Where(x => x.SeasonId < lastPrefetchedSeason)
                                         .AsEnumerable()
                                         .Select(x => x.Freeze())
                                         .ToList();

            var missingSeasonStats = await Client.GetSeasonalStatsRecordsAsync(Account, missingSeasonInfo.Select(x => (int)x.SeasonId), BoardType.Ranked | BoardType.Casual, Configuration.Get<Region>(Dragon6Setting.LegacyStatsRegion)).ConfigureAwait(false);
            var additionalSeasons = missingSeasonStats.Join(missingSeasonInfo, x => x.SeasonId, x => x.SeasonId, (s, i) => new SeasonalStatsContainer(i, s));
            var combinedSeasons = preloadedSeasons.Concat(additionalSeasons).OrderByDescending(x => x.Info.SeasonId).ToList();

            // todo add a utility that evaluates the enumerable and returns a collection-based result
            var ranked2Stats = await Client.GetSeasonalStatsAsync(Account, Account.Platform == Platform.PC ? PlatformGroup.PC : PlatformGroup.Console).ConfigureAwait(false);

            foreach (var targetSeason in combinedSeasons.Where(x => x.Stats.SeasonId == ranked2Stats.First().SeasonId))
            {
                targetSeason.Ranked2Stats = ranked2Stats.SingleOrDefault(x => x.SeasonId == targetSeason.Stats.SeasonId && x.Board == targetSeason.Stats.Board);
            }

            Boards = combinedSeasons.ToLookup(x => x.Stats.Board);
            SelectedBoard = Configuration.Get<BoardType>(Dragon6Setting.DefaultSeasonalType);
        }

        private void ChangePage(int pageDelta)
        {
            Page = Math.Clamp(Page + pageDelta, 1, MaxPage);
            SelectedSeason = null;

            InvokeAsync(StateHasChanged);
        }

        private void SetSelectedSeason(SeasonalStatsContainer stats)
        {
            // unselect if the user re-selects the card
            SelectedSeason = ReferenceEquals(stats, SelectedSeason) ? null : stats;
            InvokeAsync(StateHasChanged);
        }
    }

    public class SeasonalStatsContainer
    {
        public SeasonalStatsContainer(ISeasonInfo info, SeasonalStats stats)
        {
            Info = info;
            Stats = stats;
        }

        public ISeasonInfo Info { get; }
        public SeasonalStats Stats { get; }

        /// <summary>
        /// Ranked 2.0 stats for the season, if available
        /// </summary>
        [CanBeNull]
        public Ranked2SeasonStats Ranked2Stats { get; set; }

        /// <summary>
        /// Gets whether the player's real rank is available in this container
        /// </summary>
        public bool IsObtainedRankAvailable => Stats.Board == BoardType.Ranked && (Ranked2Stats != null || Stats.SeasonId < 28);

        public RankInfo GetDisplayRank()
        {
            var totalMatchesPlayed = Stats.Wins + Stats.Losses + Stats.Abandons;

            if (Stats.Board != BoardType.Ranked && totalMatchesPlayed > 0)
            {
                return Stats.MMRRankInfo;
            }

            // use mmr for ranks for placement matches pre-ranked 2.0
            if (Stats.Board == BoardType.Ranked && Stats.SeasonId < 28 && totalMatchesPlayed is > 0 and < 10)
            {
                return Stats.MMRRankInfo;
            }

            return Ranked2Stats?.Board == BoardType.Ranked ? Ranked2Stats.MaxRankInfo : Stats.MaxRankInfo;
        }
    }
}
