// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DragonFruit.Six.Api;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Client.Database.Entities;
using DragonFruit.Six.Client.Network.User;

namespace DragonFruit.Six.Client.Database.Services
{
    public class UserLookupCache : RealmLookupCache<Dragon6User, CachedDragon6User>
    {
        private readonly Dragon6Client _client;

        public UserLookupCache(Dragon6Client client, IMapper mapper)
            : base(mapper)
        {
            _client = client;
        }

        protected override IEnumerable<CachedDragon6User> LookupCached(IQueryable<CachedDragon6User> collection, string id, Platform platform, IdentifierType identifierType)
        {
            return collection.Where(x => x.Expires > DateTimeOffset.Now && x.ProfileId.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        protected override async Task<IEnumerable<Dragon6User>> LookupOnline(IReadOnlyCollection<string> ids, Platform platform, IdentifierType identifierType)
        {
            try
            {
                var request = new Dragon6UserRequest(ids);
                return await _client.PerformAsync<IReadOnlyCollection<Dragon6User>>(request).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return Enumerable.Empty<Dragon6User>();
            }
        }
    }
}
