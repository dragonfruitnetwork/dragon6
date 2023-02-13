// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Client.Database.Entities;
using Realms;

namespace DragonFruit.Six.Client.Database
{
    public static class RealmConfigurator
    {
        private const int SchemaVersion = 4;
        private const string RealmName = "dragon6.realm";

        public static void Initialise(IDragon6Platform devicePlatform)
        {
            var fallbackDir = Path.Combine(Path.GetTempPath(), "dragon6");
            Directory.CreateDirectory(fallbackDir);

            RealmConfiguration.DefaultConfiguration = new RealmConfiguration(Path.Combine(devicePlatform.AppData, RealmName))
            {
                SchemaVersion = SchemaVersion,
                FallbackPipePath = fallbackDir,
                MigrationCallback = PerformMigration
            };
        }

        private static void PerformMigration(Migration migration, ulong oldschemaversion)
        {
            switch (oldschemaversion)
            {
                case 1: // change dragon6 id from profile to ubisoft - clear all
                    migration.NewRealm.RemoveAll<CachedDragon6User>();
                    break;

                case 2: // change cached ubisoft account to store platform name over enum
                    migration.NewRealm.RemoveAll<CachedUbisoftAccount>();
                    break;

                case 3: // change saved/recent profiles to use string-based platform names
                    foreach (var account in migration.NewRealm.All<SavedAccount>())
                    {
                        var oldAccount = migration.OldRealm.DynamicApi.Find("saved_accounts", account.ProfileId);
                        account.PlatformName = PlatformUtils.ToPlatformName((Platform)oldAccount.platform);
                    }

                    foreach (var recent in migration.NewRealm.All<RecentAccount>())
                    {
                        var oldRecent = migration.OldRealm.DynamicApi.Find("recent_accounts", recent.ProfileId);
                        recent.PlatformName = PlatformUtils.ToPlatformName((Platform)oldRecent.platform);
                    }

                    break;
            }
        }
    }
}
