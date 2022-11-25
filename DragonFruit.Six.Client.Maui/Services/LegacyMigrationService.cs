// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Threading.Tasks;
using DragonFruit.Six.Client.Configuration;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService : ILegacyVersionMigrator
    {
        public partial bool CanRun();
        public partial Task<bool> Migrate();
    }
}
