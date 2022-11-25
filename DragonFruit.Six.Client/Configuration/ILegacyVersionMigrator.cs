// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System.Threading.Tasks;

namespace DragonFruit.Six.Client.Configuration
{
    public interface ILegacyVersionMigrator
    {
        /// <summary>
        /// Gets whether the migration is able to be run
        /// </summary>
        bool CanRun();

        /// <summary>
        /// Performs the migration, returning whether the operation was successful
        /// </summary>
        Task<bool> Migrate();
    }
}
