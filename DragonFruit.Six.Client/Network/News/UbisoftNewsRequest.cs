// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using DragonFruit.Data;
using DragonFruit.Data.Extensions;
using DragonFruit.Data.Parameters;
using JetBrains.Annotations;

namespace DragonFruit.Six.Client.Network.News
{
    public class UbisoftNewsRequest : ApiRequest
    {
        public override string Path => "https://nimbus.ubisoft.com/api/v1/items";

        public UbisoftNewsRequest()
        {
            this.WithAuthHeader("3u0FfSBUaTSew-2NVfAOSYWevVQHWtY9q3VM8Xx9Lto");
        }

        [QueryParameter("skip")]
        public int Offset { get; set; }

        [QueryParameter("limit")]
        public int Limit { get; set; } = 10;

        [UsedImplicitly]
        [QueryParameter("tags")]
        protected string Tags => "BR-rainbow-six%20GA-siege";

        [UsedImplicitly]
        [QueryParameter("categoriesFilter")]
        protected string CategoryFilter => "all";

        [UsedImplicitly]
        [QueryParameter("mediaFilter")]
        protected string MediaFilter => "news";

        [UsedImplicitly]
        [QueryParameter("locale")]
        protected string Locale => "en-US";

        [UsedImplicitly]
        [QueryParameter("fallbackLocale")]
        protected string FallbackLocale => Locale;

        [UsedImplicitly]
        [QueryParameter("environment")]
        protected string Environment => "master";
    }
}
