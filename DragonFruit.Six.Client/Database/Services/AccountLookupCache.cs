// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Client.Database.Entities;
using Realms;

namespace DragonFruit.Six.Client.Database.Services
{
    public class AccountLookupCache
    {
        private readonly IMapper _mapper;
        private readonly Dragon6Client _client;

        public AccountLookupCache(Dragon6Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        /// <summary>
        /// Performs an account lookup for profiles on matching platforms. Writes accounts to a cache to reduce network requests for duplicate data
        /// </summary>
        /// <param name="ids">A collection of identifiers to lookup</param>
        /// <param name="platform">The <see cref="Platform"/> the users are on</param>
        /// <param name="type">The <see cref="IdentifierType"/> to <see cref="ids"/> represent</param>
        public async Task<IReadOnlyList<UbisoftAccount>> LookupAsync(IReadOnlyCollection<string> ids, Platform platform, IdentifierType type)
        {
            var missingIds = new List<string>(ids);
            var resultantAccounts = new List<UbisoftAccount>(ids.Count);

            // check realm first
            using (var readRealm = await Realm.GetInstanceAsync())
            {
                var table = readRealm.All<CachedUbisoftAccount>().Where(x => x.Expires > DateTimeOffset.Now && x.PlatformValue == (int)platform);

                foreach (var id in ids)
                {
                    var cachedAccounts = type switch
                    {
                        IdentifierType.Name => table.Where(x => x.Username.Equals(id, StringComparison.OrdinalIgnoreCase)),
                        IdentifierType.UserId => table.Where(x => x.UbisoftId.Equals(id, StringComparison.OrdinalIgnoreCase)),
                        IdentifierType.PlatformId => table.Where(x => x.PlatformId.Equals(id, StringComparison.OrdinalIgnoreCase)),

                        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                    };

                    if (cachedAccounts.Any())
                    {
                        missingIds.Remove(id);
                        resultantAccounts.AddRange(cachedAccounts.AsEnumerable().Select(_mapper.Map<UbisoftAccount>));
                    }
                }
            }

            if (!missingIds.Any())
            {
                return resultantAccounts;
            }

            IEnumerable<UbisoftAccount> discoveredAccounts;

            try
            {
                discoveredAccounts = await _client.GetAccountsAsync(missingIds, platform, type).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (missingIds.Count > 1)
            {
                // one or more accounts may be invalid, split into separate lookups
                var lookupTasks = missingIds.Select(x => _client.GetAccountAsync(x, platform, type));
                discoveredAccounts = await Task.WhenAll(lookupTasks).ConfigureAwait(false);
            }

            using var writeRealm = await Realm.GetInstanceAsync();
            using var transaction = await writeRealm.BeginWriteAsync();

            foreach (var account in discoveredAccounts)
            {
                writeRealm.Add(_mapper.Map<CachedUbisoftAccount>(account), true);
                resultantAccounts.Add(account);
            }

            await transaction.CommitAsync().ConfigureAwait(false);

            return resultantAccounts;
        }
    }
}
