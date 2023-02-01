// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using DragonFruit.Six.Client.Database.Entities;
using Realms;

namespace DragonFruit.Six.Client.Database
{
    public static class RealmConfigurator
    {
        private const int SchemaVersion = 2;
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
            }
        }
    }
}
