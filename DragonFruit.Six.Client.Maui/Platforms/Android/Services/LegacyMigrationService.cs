// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

// ReSharper disable once CheckNamespace
namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService
    {
        private static string Preferences => Path.Combine(FileSystem.AppDataDirectory, "preferences.ini");
        private static string Database => Path.Combine(FileSystem.AppDataDirectory, "dragon6.db");

        public partial bool CanRun() => false;

        public partial Task<bool> Migrate()
        {
            if (File.Exists(Preferences))
            {
                MigrateCommonPreferences(Preferences, out _, out _);
                // dragon6 mobile has no additional preferences
            }

            return Task.FromResult(true);
        }
    }
}
