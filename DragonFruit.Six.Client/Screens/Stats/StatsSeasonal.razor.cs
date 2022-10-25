// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Api.Seasonal.Entities;
using DragonFruit.Six.Api.Seasonal.Enums;
using Microsoft.AspNetCore.Components;
using Realms;
using SeasonInfo = DragonFruit.Six.Client.Database.Entities.SeasonInfo;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class StatsSeasonal
    {
        private const bool DisplayMaxRank = true;
        private const int EntriesPerPage = 4;

        [Inject]
        public Dragon6Client Client { get; set; }

        [Parameter]
        public UbisoftAccount Account { get; set; }

        [Parameter]
        public IEnumerable<SeasonalStats> PreloadedStats { get; set; }

        private BoardType SelectedBoard { get; set; } = BoardType.Ranked;
        private SeasonalStatsContainer SelectedSeason { get; set; }
        private ILookup<BoardType, SeasonalStatsContainer> Boards { get; set; }

        private int Page { get; set; }
        private int MaxPage => (Boards[SelectedBoard].Count() - 1) / EntriesPerPage + 1;

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

            // because realm can't process Contains(), we need to use the smallest season id we retrieved and go backwards from there.
            var lastPrefetchedSeason = PreloadedStats.Min(y => y.SeasonId);
            var missingSeasonInfo = realm.All<SeasonInfo>()
                                         .Where(x => x.SeasonId < lastPrefetchedSeason)
                                         .AsEnumerable()
                                         .Select(x => x.Freeze())
                                         .ToList();

            var missingSeasonStats = await Client.GetSeasonalStatsRecordsAsync(Account, missingSeasonInfo.Select(x => (int)x.SeasonId), BoardType.Ranked | BoardType.Casual, Region.EMEA).ConfigureAwait(false);
            var additionalSeasons = missingSeasonStats.Join(missingSeasonInfo, x => x.SeasonId, x => x.SeasonId, (s, i) => new SeasonalStatsContainer(i, s));

            Boards = preloadedSeasons.Concat(additionalSeasons).OrderByDescending(x => x.Info.SeasonId).ToLookup(x => x.Stats.Board);
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

        private void SwapDisplayedBoard()
        {
            Page = 1;
            SelectedSeason = null;
            SelectedBoard = SelectedBoard switch
            {
                BoardType.Casual => BoardType.Ranked,
                BoardType.Ranked => BoardType.Casual,

                _ => SelectedBoard
            };

            InvokeAsync(StateHasChanged);
        }
    }

    public class SeasonalStatsContainer
    {
        public SeasonalStatsContainer(SeasonInfo info, SeasonalStats stats)
        {
            Info = info;
            Stats = stats;
        }

        public SeasonInfo Info { get; set; }
        public SeasonalStats Stats { get; set; }
    }
}
