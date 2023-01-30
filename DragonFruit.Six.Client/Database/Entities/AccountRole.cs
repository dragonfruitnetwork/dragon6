// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Database.Entities
{
    public enum AccountRole
    {
        BlockedByAdmin = -2,
        BlockedBySelf = -1,

        Normal = 0,

        Beta = 1,
        Translator = 2,
        Verified = 3,
        Supporter = 4,
        Contributor = 5,
        Developer = 6
    }
}
