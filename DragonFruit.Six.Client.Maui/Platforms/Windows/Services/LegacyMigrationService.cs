﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Client.Configuration;
using DragonFruit.Six.Client.Database;
using DragonFruit.Six.Client.Database.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Realms;

// ReSharper disable once CheckNamespace
namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService
    {
        private static string BasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DragonFruit Network", "Dragon6", "Electron");
        private static string Preferences => Path.Combine(BasePath, "preferences.ini");
        private static string Database => Path.Combine(BasePath, "master.db");

        public partial bool CanRun() => File.Exists(Database);

        public async partial Task<bool> Migrate()
        {
            // migrate preferences
            if (File.Exists(Preferences))
            {
                MigrateCommonPreferences(Preferences, out var config, out var preferences);

                config.Set(Dragon6Setting.EnableDiscordRPC, preferences["discord_rpc"]!.ToObject<bool>());
                config.Set(Dragon6Setting.DefaultSeasonalType, preferences["casual_ranked"]!.ToObject<bool>() ? BoardType.Casual : BoardType.Ranked);
            }

            if (File.Exists(Database))
            {
                using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var connection = new SqliteConnection($"Data Source={Database};Mode=ReadOnly");

                await connection.OpenAsync(cancellation.Token).ConfigureAwait(false);
                connection.Disposed += (sender, _) =>
                {
                    ((SqliteConnection)sender!).Close();

                    // RCWs need to be collected to fully close the file - see https://stackoverflow.com/a/8513453
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Directory.Delete(BasePath, true);
                };

                var databaseVersionSupported = await connection.QuerySingleAsync<int>("SELECT COUNT(1) FROM __EFMigrationsHistory WHERE MigrationId = '20211229181818_RenameTables'").ConfigureAwait(false);

                if (databaseVersionSupported == 0)
                {
                    // database was too old, create a copy and then let it get deleted...
                    // todo log failure
                    File.Copy(Database, Path.Combine(_services.GetRequiredService<IDragon6Platform>().AppData, "dragon6.master.old.db"));
                    return false;
                }

                // migrate accounts
                var legacySavedPlayers = await connection.QueryAsync("SELECT * FROM accs_saved").ConfigureAwait(false);
                var legacyRecentPlayers = await connection.QueryAsync("SELECT * FROM accs_recent").ConfigureAwait(false);

                using var realm = await Realm.GetInstanceAsync(cancellationToken: cancellation.Token);
                using var transaction = await realm.BeginWriteAsync(cancellation.Token);

                foreach (var player in legacySavedPlayers)
                {
                    realm.Add(new SavedAccount
                    {
                        ProfileId = player.profile_id,
                        UbisoftId = player.ubisoft_id,
                        Platform = (Platform)player.platform,

                        SavedAt = DateTimeOffset.Parse(player.account_added),
                        LastStatsUpdate = DateTimeOffset.MinValue
                    });
                }

                foreach (var player in legacyRecentPlayers)
                {
                    realm.Add(new RecentAccount
                    {
                        ProfileId = player.profile_id,
                        UbisoftId = player.ubisoft_id,
                        Platform = (Platform)player.platform,

                        Username = player.account_name,
                        LastSearched = DateTimeOffset.Parse(player.last_searched_at)
                    });
                }

                await transaction.CommitAsync(cancellation.Token);
            }

            return true;
        }
    }
}