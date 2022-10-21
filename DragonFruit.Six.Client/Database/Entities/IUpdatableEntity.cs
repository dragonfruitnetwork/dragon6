// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;

namespace DragonFruit.Six.Client.Database.Entities
{
    public interface IUpdatableEntity
    {
        DateTimeOffset LastUpdated { get; set; }
    }
}
