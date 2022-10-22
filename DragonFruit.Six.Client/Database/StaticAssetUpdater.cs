// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DragonFruit.Data;
using DragonFruit.Data.Extensions;
using DragonFruit.Data.Serializers;
using DragonFruit.Six.Api;
using DragonFruit.Six.Client.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using Realms;

namespace DragonFruit.Six.Client.Database
{
    public static class StaticAssetUpdater
    {
        public static async Task UpdateTable<T>(IServiceProvider services, Func<ApiRequest> requestFactory) where T : RealmObject, IUpdatableEntity
        {
            var client = services.GetRequiredService<Dragon6Client>();
            var request = requestFactory.Invoke();
            var emptyCollection = true;

            // async methods need to have different realms after await usages (because we don't guarantee returning to the same thread)
            using var realm = await Realm.GetInstanceAsync().ConfigureAwait(true);

            if (realm.All<T>().Any())
            {
                emptyCollection = false;

                var latestUpdate = realm.All<T>().First().LastUpdated;
                request.WithHeader("If-Modified-Since", latestUpdate.ToString("R"));
            }

            using var response = await client.PerformAsync(request);

            if (response.StatusCode is HttpStatusCode.OK)
            {
                var date = DateTime.UtcNow;
                var responseStream = await response.Content.ReadAsStreamAsync();
                var elements = client.Serializer.Resolve<T>(DataDirection.In).Deserialize<IList<T>>(responseStream);

                foreach (var element in elements)
                {
                    element.LastUpdated = date;
                }

                await realm.WriteAsync(() =>
                {
                    realm.Add(elements, true);

                    if (!emptyCollection)
                    {
                        realm.RemoveRange(realm.All<T>().Where(x => x.LastUpdated < date));
                    }
                });
            }
        }
    }
}
