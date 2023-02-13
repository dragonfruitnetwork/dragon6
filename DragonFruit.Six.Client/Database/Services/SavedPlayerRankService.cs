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
using DragonFruit.Six.Api.Enums;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Client.Database.Entities;
using Realms;

namespace DragonFruit.Six.Client.Database.Services
{
    public class SavedPlayerRankService : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Dragon6Client _client;

        private IDisposable _notificationListener;

        public SavedPlayerRankService(Dragon6Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public void StartService()
        {
            _notificationListener ??= Realm.GetInstance().All<SavedAccount>().SubscribeForNotifications(DatabaseChangeOccured);
        }

        private void DatabaseChangeOccured(IRealmCollection<SavedAccount> sender, ChangeSet changes, Exception error)
        {
            if (changes == null && sender.Any())
            {
                // do full update check
                _ = UpdateAccounts(sender.Where(x => x.LastStatsUpdate < DateTimeOffset.Now.AddHours(-6)).ToList());
            }
            else if (changes?.InsertedIndices.Any() == true)
            {
                // process new accounts
                _ = UpdateAccounts(changes.InsertedIndices.Select(x => sender[x]).ToList());
            }
        }

        private async Task UpdateAccounts(IReadOnlyCollection<SavedAccount> accounts)
        {
            if (!accounts.Any())
            {
                return;
            }

            var statsDownloadDate = DateTimeOffset.UtcNow;
            var seasonalStatsTasks = accounts.Select(x => _mapper.Map<UbisoftAccount>(x))
                                             .GroupBy(x => x.Platform == Platform.PC)
                                             .Select(x => _client.GetSeasonalStatsAsync(x, x.Key ? PlatformGroup.PC : PlatformGroup.Console));

            var seasonalStats = await Task.WhenAll(seasonalStatsTasks).ConfigureAwait(false);

            using var realm = await Realm.GetInstanceAsync();
            using var transaction = await realm.BeginWriteAsync();

            foreach (var profile in seasonalStats.SelectMany(x => x).Where(x => x.Board == BoardType.Ranked).GroupBy(x => x.ProfileId))
            {
                var targetAccount = realm.Find<SavedAccount>(profile.Key);

                targetAccount.SeasonMaxRank = profile.Max(x => x.MaxRank);
                targetAccount.LastStatsUpdate = statsDownloadDate;
            }

            await transaction.CommitAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _notificationListener?.Dispose();
            _notificationListener = null;
        }
    }
}
