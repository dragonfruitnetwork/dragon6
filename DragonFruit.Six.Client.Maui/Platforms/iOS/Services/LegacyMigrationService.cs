﻿// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

// ReSharper disable CheckNamespace

using System;
using System.Threading.Tasks;

namespace DragonFruit.Six.Client.Maui.Services
{
    public partial class LegacyMigrationService
    {
        public partial bool CanRun() => false;
        public partial Task<bool> Migrate() => throw new PlatformNotSupportedException();
    }
}
