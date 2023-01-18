// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Enums;
using DragonFruit.Six.Api.Modern;
using DragonFruit.Six.Api.Modern.Entities;
using DragonFruit.Six.Api.Modern.Enums;
using DragonFruit.Six.Client.Database.Entities;
using Havit.Linq;
using Microsoft.AspNetCore.Components;
using Realms;

namespace DragonFruit.Six.Client.Screens.Stats
{
    public partial class StatsOperator
    {
        [Parameter]
        public UbisoftAccount Account { get; set; }

        [Inject]
        private Dragon6Client Client { get; set; }

        private bool UserFilterUnplayed { get; set; }
        private bool SystemFilterUnplayed { get; set; }

        private OperatorSortMode SortMode { get; set; } = OperatorSortMode.RoundsPlayed;

        private IReadOnlyCollection<OperatorStatsContainer> Stats { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var operatorStatsContainer = await Client.GetModernOperatorStatsAsync(Account, PlaylistType.Independent, startDate: DateTime.UtcNow.AddDays(-Screens.Stats.Stats.ModernStatsRange)).ConfigureAwait(false);
            var operatorStats = operatorStatsContainer.AllModes.AsAttacker.Concat(operatorStatsContainer.AllModes.AsDefender);

            using var realm = await Realm.GetInstanceAsync();

            // because realm entities are locked to the thread the realm was created on, the items need to be frozen to allow read-only access on render threads.
            Stats = realm.All<OperatorInfo>().AsEnumerable().LeftJoin(operatorStats, x => x.Id, x => x.Name.ToLowerInvariant(), (info, stats) => new OperatorStatsContainer(info.Freeze(), stats)).ToList();
        }

        private IEnumerable<OperatorStatsContainer> GetDisplayableGroup(IEnumerable<OperatorStatsContainer> stats)
        {
            // perform filtering first, then partition based on whether the operator has been played
            var filteredGroups = UserFilterUnplayed || SystemFilterUnplayed ? stats.Where(x => x.Stats?.RoundsPlayed > 0) : stats;

            if (SortMode is OperatorSortMode.Default)
            {
                return filteredGroups;
            }

            var partitionedGroups = filteredGroups.ToLookup(x => x.Stats != null);
            var orderedGroup = SortMode switch
            {
                OperatorSortMode.Kd => partitionedGroups[true].OrderByDescending(x => x.Stats.Kd),
                OperatorSortMode.Wl => partitionedGroups[true].OrderByDescending(x => x.Stats.RoundWl),
                OperatorSortMode.RoundsPlayed => partitionedGroups[true].OrderByDescending(x => x.Stats.RoundsPlayed),

                _ => throw new ArgumentOutOfRangeException()
            };

            return orderedGroup.Concat(partitionedGroups[false]);
        }

        private enum OperatorSortMode
        {
            Default,
            RoundsPlayed,

            [Description("K/D")]
            Kd,

            [Description("W/L")]
            Wl
        }

        private class OperatorStatsContainer
        {
            public OperatorStatsContainer(OperatorInfo info, ModernOperatorStats stats)
            {
                Info = info;
                Stats = stats;
            }

            public OperatorInfo Info { get; set; }
            public ModernOperatorStats Stats { get; set; }
        }
    }
}
