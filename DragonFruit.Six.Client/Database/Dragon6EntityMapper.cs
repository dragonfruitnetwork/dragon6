﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using AutoMapper;
using DragonFruit.Six.Api.Accounts.Entities;
using DragonFruit.Six.Client.Database.Entities;
using DragonFruit.Six.Client.Network.User;

namespace DragonFruit.Six.Client.Database
{
    internal static class Dragon6EntityMapper
    {
        public static void ConfigureMapper(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Dragon6User, CachedDragon6User>().ReverseMap();
            cfg.CreateMap<Dragon6UserCovers, CachedDragon6UserCoverSources>().ReverseMap();

            cfg.CreateMap<UbisoftAccount, CachedUbisoftAccount>().ReverseMap();
            cfg.CreateMap<UbisoftAccount, SavedAccount>().ForMember(x => x.SavedAt, x => x.UseDestinationValue()).ReverseMap();
            cfg.CreateMap<UbisoftAccount, RecentAccount>().ForMember(x => x.LastSearched, x => x.UseDestinationValue());
        }
    }
}
