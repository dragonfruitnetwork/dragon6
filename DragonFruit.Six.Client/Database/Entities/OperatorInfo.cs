// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using DragonFruit.Six.Api.Enums;
using Newtonsoft.Json;
using Realms;

namespace DragonFruit.Six.Client.Database.Entities
{
    [MapTo("operators")]
    [JsonObject(MemberSerialization.OptIn)]
    public class OperatorInfo : RealmObject, IUpdatableEntity, IOperatorInfo
    {
        [PrimaryKey]
        [JsonProperty("id")]
        [MapTo("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        [MapTo("name")]
        public string Name { get; set; }

        [JsonProperty("org")]
        [MapTo("organisation")]
        public string Organisation { get; set; }

        [JsonProperty("sub")]
        [MapTo("sub")]
        public string Subtitle { get; set; }

        [Ignored]
        [JsonProperty("type")]
        public OperatorType Type
        {
            get => (OperatorType)TypeValue;
            set => TypeValue = (byte)value;
        }

        [MapTo("type")]
        private byte TypeValue { get; set; }

        [JsonProperty("ord")]
        [MapTo("order")]
        public int Order { get; set; }

        [MapTo("last_update")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
