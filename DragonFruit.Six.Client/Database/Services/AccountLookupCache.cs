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

namespace DragonFruit.Six.Client.Database.Services
{
    public class AccountLookupCache : RealmLookupCache<UbisoftAccount, CachedUbisoftAccount>
    {
        private readonly Dragon6Client _client;

        public AccountLookupCache(Dragon6Client client, IMapper mapper)
            : base(mapper)
        {
            _client = client;
        }

        protected override IEnumerable<CachedUbisoftAccount> LookupCached(IQueryable<CachedUbisoftAccount> collection, string id, Platform platform, IdentifierType identifierType)
        {
            return identifierType switch
            {
                IdentifierType.Name => collection.Where(x => x.Expires > DateTimeOffset.Now && x.Username.Equals(id, StringComparison.OrdinalIgnoreCase)),
                IdentifierType.UserId => collection.Where(x => x.Expires > DateTimeOffset.Now && x.UbisoftId.Equals(id, StringComparison.OrdinalIgnoreCase)),
                IdentifierType.PlatformId => collection.Where(x => x.Expires > DateTimeOffset.Now && x.PlatformId.Equals(id, StringComparison.OrdinalIgnoreCase)),

                _ => throw new ArgumentOutOfRangeException(nameof(identifierType), identifierType, null)
            };
        }

        protected override async Task<IEnumerable<UbisoftAccount>> LookupOnline(IReadOnlyCollection<string> ids, Platform platform, IdentifierType identifierType)
        {
            IEnumerable<UbisoftAccount> discoveredAccounts;

            try
            {
                discoveredAccounts = await _client.GetAccountsAsync(ids, platform, identifierType).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (ids.Count > 1)
            {
                // one or more accounts may be invalid, split into separate lookups
                var lookupTasks = ids.Select(x => _client.GetAccountAsync(x, platform, identifierType));
                discoveredAccounts = await Task.WhenAll(lookupTasks).ConfigureAwait(false);
            }

            return discoveredAccounts;
        }
    }
}
