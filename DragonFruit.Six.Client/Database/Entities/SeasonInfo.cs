// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Newtonsoft.Json;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("seasons")]
    [JsonObject(MemberSerialization.OptIn)]
    public class SeasonInfo : RealmObject, IUpdatableEntity
    {
        [PrimaryKey]
        [JsonProperty("id")]
        [MapTo("season_id")]
        public byte SeasonId { get; set; }

        [JsonProperty("operation")]
        [MapTo("season_name")]
        public string SeasonName { get; set; }

        [JsonProperty("accent")]
        [MapTo("accent")]
        public string AccentColour { get; set; }

        [JsonProperty("year")]
        [MapTo("year")]
        public int Year { get; set; }

        [JsonProperty("season")]
        [MapTo("season")]
        public byte Season { get; set; }

        [JsonIgnore]
        [MapTo("last_update")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
