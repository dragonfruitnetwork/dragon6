// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Six.Api.Enums;

namespace DragonFruit.Six.Client.Database.Entities
{
    public interface IOperatorInfo
    {
        string Id { get; set; }
        string Name { get; set; }
        string Organisation { get; set; }
        string Subtitle { get; set; }
        OperatorType Type { get; set; }
        int Order { get; set; }
    }
}
