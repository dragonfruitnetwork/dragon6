// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Database.Entities
{
    public interface ISeasonInfo
    {
        int SeasonId { get; set; }
        string SeasonName { get; set; }
        string AccentColour { get; set; }

        int Year { get; set; }
        int Season { get; set; }
    }
}
