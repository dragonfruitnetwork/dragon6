// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Seasonal;
using DragonFruit.Six.Client.Database.Entities;
using Realms;

namespace DragonFruit.Six.Client.Database.Services
{
    public class SavedPlayerRankService : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Dragon6Client _client;

        private Realm _realm;
        private IDisposable _notificationListener;

        public SavedPlayerRankService(Dragon6Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public void StartService()
        {
            _realm ??= Realm.GetInstance();
            _notificationListener ??= _realm.All<SavedAccount>().SubscribeForNotifications(DatabaseChangeOccured);
        }

        private void DatabaseChangeOccured(IRealmCollection<SavedAccount> sender, ChangeSet changes, Exception error)
        {
            if (changes == null && sender.Any())
            {
                // do full update check
                _ = UpdateAccounts(sender.Where(x => x.LastStatsUpdate < DateTimeOffset.Now.AddHours(-6)));
            }
            else if (changes.InsertedIndices.Any())
            {
                // process new accounts
                _ = UpdateAccounts(changes.InsertedIndices.Select(x => sender[x]));
            }
        }

        private async Task UpdateAccounts(IEnumerable<SavedAccount> accounts)
        {
            var mappedAccounts = accounts.Select(x => _mapper.Map<UbisoftAccount>(x));
            var seasonalStats = await _client.GetSeasonalStatsAsync(mappedAccounts).ConfigureAwait(false);

            using var realm = await Realm.GetInstanceAsync();

            // ReSharper disable once MethodHasAsyncOverload
            realm.Write(() =>
            {
                foreach (var (profileId, stats) in seasonalStats.Stats)
                {
                    var targetAccount = realm.Find<SavedAccount>(profileId);
                    targetAccount.SeasonMaxRank = stats.MaxRank;
                }
            });
        }

        public void Dispose()
        {
            _realm?.Dispose();
            _notificationListener?.Dispose();

            _realm = null;
            _notificationListener = null;
        }
    }
}
