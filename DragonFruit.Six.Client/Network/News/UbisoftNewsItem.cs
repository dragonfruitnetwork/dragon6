// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using Newtonsoft.Json;

namespace DragonFruit.Six.Client.Network.News
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class UbisoftNewsItem
    {
        /// <summary>
        /// Alphanumeric identifier for the post.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Item type (usually "news")
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Matching tag (used in filtering)
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Matching categories
        /// </summary>
        [JsonProperty("categories")]
        public string[] Categories { get; set; }

        /// <summary>
        /// Article publish date (currently in 'R' format, with extra stuff that JSON.NET can't handle)
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Article title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Plain-text abstract for the news post
        /// </summary>
        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// Markdown content of the news article
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Read time of the article, in minutes
        /// </summary>
        [JsonProperty("readTime")]
        public float ReadTime { get; set; }

        /// <summary>
        /// Article url-related information.
        /// </summary>
        [JsonProperty("button")]
        public UbisoftNewsItemButton Link { get; set; }

        /// <summary>
        /// Associated image displayed on the with the article
        /// </summary>
        [JsonProperty("thumbnail")]
        public UbisoftNewsItemImageInfo ImageThumbnail { get; set; }
    }
}
