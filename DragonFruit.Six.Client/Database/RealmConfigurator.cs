// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using Realms;

namespace DragonFruit.Six.Client.Database
{
    public static class RealmConfigurator
    {
        private const int SchemaVersion = 1;
        private const string RealmName = "dragon6.realm";

        public static void Initialise(IFileSystemStructure fileSystemStructure)
        {
            var fallbackDir = Path.Combine(Path.GetTempPath(), "dragon6");
            Directory.CreateDirectory(fallbackDir);

            RealmConfiguration.DefaultConfiguration = new RealmConfiguration(Path.Combine(fileSystemStructure.AppData, RealmName))
            {
                SchemaVersion = SchemaVersion,
                FallbackPipePath = fallbackDir,
                MigrationCallback = PerformMigration
            };
        }

        private static void PerformMigration(Migration migration, ulong oldschemaversion)
        {
            // no migrations yet...
        }
    }
}
