// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

namespace DragonFruit.Six.Client.Database.Entities
{
    public interface ISeasonInfo
    {
        byte SeasonId { get; set; }
        string SeasonName { get; set; }
        string AccentColour { get; set; }
        int Year { get; set; }
        byte Season { get; set; }
    }
}
