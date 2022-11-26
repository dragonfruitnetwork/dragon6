// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.IO;
using System.Threading.Tasks;
using DragonFruit.Data.Serializers.Newtonsoft;
using DragonFruit.Six.Api.Accounts.Enums;
using DragonFruit.Six.Api.Seasonal.Enums;
using DragonFruit.Six.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService : ILegacyVersionMigrator
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ILegacyVersionMigrator> _logger;

        public LegacyMigrationService(IServiceProvider services, ILogger<ILegacyVersionMigrator> logger)
        {
            _services = services;
            _logger = logger;
        }

        public partial bool CanRun();
        public partial Task<bool> Migrate();

        private void MigrateCommonPreferences(string path, out Dragon6Configuration config, out JObject preferences)
        {
            preferences = FileServices.ReadFile<JObject>(path);
            config = _services.GetRequiredService<Dragon6Configuration>();

            config.Set(Dragon6Setting.LegacyStatsRegion, preferences["region"]!.ToObject<Region>());
            config.Set(Dragon6Setting.DefaultPlatform, preferences["platform"]!.ToObject<Platform>());

            File.Delete(path);
        }
    }
}
