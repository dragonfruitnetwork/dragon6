// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DragonFruit.Six.Api.Accounts.Enums;
using Realms;

namespace DragonFruit.Six.Client.Database.Services
{
    public abstract class RealmLookupCache<T, TRealm> where TRealm : RealmObject
    {
        private readonly IMapper _mapper;

        protected RealmLookupCache(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Performs an account lookup for profiles on matching platforms. Writes accounts to a cache to reduce network requests for duplicate data
        /// </summary>
        /// <param name="ids">A collection of identifiers to lookup</param>
        /// <param name="platform">The <see cref="Platform"/> the users are on</param>
        /// <param name="type">The <see cref="IdentifierType"/> to <see cref="ids"/> represent</param>
        public async Task<IReadOnlyList<T>> LookupAsync(IReadOnlyCollection<string> ids, Platform platform, IdentifierType type)
        {
            var missingIds = new List<string>(ids);
            var resultantAccounts = new List<T>(ids.Count);

            // check realm first
            using (var readRealm = await Realm.GetInstanceAsync())
            {
                foreach (var id in ids)
                {
                    var cachedAccounts = LookupCached(readRealm.All<TRealm>(), id, platform, type);

                    if (cachedAccounts?.Any() == true)
                    {
                        missingIds.Remove(id);
                        resultantAccounts.AddRange(cachedAccounts.Select(_mapper.Map<T>));
                    }
                }
            }

            if (!missingIds.Any())
            {
                return resultantAccounts;
            }

            var discoveredAccounts = await LookupOnline(missingIds, platform, type).ConfigureAwait(false);

            using var writeRealm = await Realm.GetInstanceAsync();
            using var transaction = await writeRealm.BeginWriteAsync();

            foreach (var account in discoveredAccounts)
            {
                writeRealm.Add(_mapper.Map<TRealm>(account), true);
                resultantAccounts.Add(account);
            }

            await transaction.CommitAsync().ConfigureAwait(false);
            return resultantAccounts;
        }

        protected abstract IEnumerable<TRealm> LookupCached(IQueryable<TRealm> collection, string id, Platform platform, IdentifierType identifierType);
        protected abstract Task<IEnumerable<T>> LookupOnline(IReadOnlyCollection<string> ids, Platform platform, IdentifierType identifierType);
    }
}
