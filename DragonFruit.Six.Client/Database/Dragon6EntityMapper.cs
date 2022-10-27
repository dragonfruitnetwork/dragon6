// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using AutoMapper;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Client.Database.Entities;

namespace DragonFruit.Six.Client.Database
{
    public static class Dragon6EntityMapper
    {
        public static void ConfigureMapper(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UbisoftAccount, RecentAccount>()
               .ForMember(x => x.LastSearched, x => x.UseDestinationValue())
               .ReverseMap();
        }
    }
}
