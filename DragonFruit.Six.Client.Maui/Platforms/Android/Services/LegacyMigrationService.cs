// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Client.Database.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using Realms;

// ReSharper disable once CheckNamespace
namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService
    {
        private static string Database => Path.Combine(FileSystem.AppDataDirectory, "dragon6.db");
        private static string Preferences => Path.Combine(FileSystem.AppDataDirectory, "preferences.ini");
        private static string DeletionMarker => Path.Combine(FileSystem.AppDataDirectory, ".marked-for-deletion");

        public partial bool CanRun() => File.Exists(Database) || File.Exists(DeletionMarker);

        public async partial Task<bool> Migrate()
        {
            if (File.Exists(DeletionMarker))
            {
                File.Delete(DeletionMarker);

                foreach (var file in Directory.EnumerateFiles(FileSystem.AppDataDirectory, "dragon6.db*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }

                return true;
            }

            if (File.Exists(Preferences))
            {
                // dragon6 mobile has no additional preferences
                MigrateCommonPreferences(Preferences, out _, out _);
            }

            if (File.Exists(Database))
            {
                _logger.LogInformation("Beginning database migration...");

                using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var connection = new SqliteConnection($"Data Source={Database};Mode=ReadOnly");

                try
                {
                    await connection.OpenAsync(cancellation.Token).ConfigureAwait(false);
                }
                catch (IOException e)
                {
                    _logger.LogInformation("Database connection failed: {message}", e.Message);
                    return false;
                }

                connection.Disposed += (sender, _) =>
                {
                    _logger.LogInformation("Database migration ended, deleting database...");

                    ((SqliteConnection)sender!).Close();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    File.Create(DeletionMarker).DisposeAsync();
                };

                // migrate accounts
                var legacySavedPlayers = await connection.QueryAsync("SELECT rowid, * FROM saved_players").ConfigureAwait(false);
                var legacyRecentPlayers = await connection.QueryAsync("SELECT rowid, * FROM recent_players").ConfigureAwait(false);

                using var realm = await Realm.GetInstanceAsync(cancellationToken: cancellation.Token);
                using var transaction = await realm.BeginWriteAsync(cancellation.Token);

                foreach (var player in legacySavedPlayers)
                {
                    realm.Add(new SavedAccount
                    {
                        ProfileId = player.profile_id,
                        UbisoftId = player.ubisoft_id,
                        Platform = (Platform)player.platform,

                        LastStatsUpdate = DateTimeOffset.MinValue,
                        SavedAt = new DateTimeOffset(new DateTime(2021, 01, 01).AddDays(player.rowid), TimeSpan.Zero)
                    });
                }

                foreach (var player in legacyRecentPlayers)
                {
                    realm.Add(new RecentAccount
                    {
                        ProfileId = player.profile_id,
                        UbisoftId = player.ubisoft_id,
                        Platform = (Platform)player.platform,

                        Username = player.username,
                        LastSearched = new DateTimeOffset(DateTime.Parse(player.last_searched), TimeSpan.Zero)
                    });
                }

                await transaction.CommitAsync(cancellation.Token);
            }

            return true;
        }
    }
}
