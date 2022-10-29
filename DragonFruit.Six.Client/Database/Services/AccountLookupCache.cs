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
        public async IAsyncEnumerable<UbisoftAccount> LookupAsync(IReadOnlyCollection<string> ids, Platform platform, IdentifierType type)
        {
            var missingIds = ids.ToList();

            // check realm first
            using (var realm = await Realm.GetInstanceAsync())
            {
                var table = realm.All<CachedUbisoftAccount>().Where(x => x.Expires < DateTimeOffset.Now && x.PlatformValue == (int)platform);

                foreach (var id in ids)
                {
                    var discoveredAccount = type switch
                    {
                        IdentifierType.Name => table.SingleOrDefault(x => x.Username.Equals(id, StringComparison.OrdinalIgnoreCase)),
                        IdentifierType.UserId => table.SingleOrDefault(x => x.UbisoftId.Equals(id)),
                        IdentifierType.PlatformId => table.SingleOrDefault(x => x.PlatformId.Equals(id)),

                        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                    };

                    if (discoveredAccount != null)
                    {
                        missingIds.Remove(id);
                        yield return _mapper.Map<UbisoftAccount>(discoveredAccount);
                    }
                }
            }

            if (!missingIds.Any())
            {
                yield break;
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

            if (discoveredAccounts.Any())
            {
                using var realm = await Realm.GetInstanceAsync();
                using var transaction = await realm.BeginWriteAsync();

                foreach (var account in discoveredAccounts)
                {
                    realm.Add(_mapper.Map<CachedUbisoftAccount>(account));
                    yield return account;
                }

                await transaction.CommitAsync();
            }
        }
    }
}
